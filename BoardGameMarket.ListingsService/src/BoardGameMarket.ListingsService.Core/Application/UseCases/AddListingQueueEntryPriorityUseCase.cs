using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases
{
	public class AddListingQueueEntryPriorityUseCase
	{
		private readonly IListingReadService _listingReadService;
		private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
		private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;
		private readonly IUnitOfWork _unitOfWork;

		public AddListingQueueEntryPriorityUseCase(IListingReadService listingReadService, IListingQueueEntryReadService listingQueueEntryReadService, IListingQueueEntryWriteService listingQueueEntryWriteService, IUnitOfWork unitOfWork)
		{
			_listingReadService = listingReadService;
			_listingQueueEntryReadService = listingQueueEntryReadService;
			_listingQueueEntryWriteService = listingQueueEntryWriteService;
			_unitOfWork = unitOfWork;
		}

		// TODO: Through SQL atomic concurrency safe
		public async Task<ListingQueueEntryInfo> ExecuteAsync(Guid listingId, Guid requestedQueueEntryId)
		{
			try
			{
				await _unitOfWork.BeginTransactionAsync();

				Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
				if (listing == null)
				{
					throw new KeyNotFoundException($"Listing {listingId} not found");
				}

				ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
				{
					ListingIds = [listingId],
				};
				IEnumerable<ListingQueueEntry> queue = (await _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter)).OrderByDescending(qe => qe.Priority);

				ListingQueueEntry? requestedPriorityEntry = queue.Where(qe => qe.Id == requestedQueueEntryId).FirstOrDefault();

				if (requestedPriorityEntry == null)
				{
					throw new KeyNotFoundException($"QueueEntry {requestedQueueEntryId} not found");
				}

				if (requestedPriorityEntry.ListingId != listingId)
				{
					throw new InvalidOperationException("Mismatch queueEntry and listing");
				}

				if (requestedPriorityEntry.Priority != null)
				{
					await _unitOfWork.CommitTransactionAsync();
					return requestedPriorityEntry.ToListingQueueEntryInfo();
				}

				ListingQueueEntry? lastPriorityEntry = queue.FirstOrDefault();

				// If no priorities exist in queue
				if (lastPriorityEntry == null || lastPriorityEntry.Priority == null)
				{
					requestedPriorityEntry.Priority = QueueDomainService.INITIAL_PRIORITY;
				}
				else
				{
					requestedPriorityEntry.Priority = lastPriorityEntry.Priority + QueueDomainService.PRIORITY_GAP;
				}

				if (await _listingQueueEntryWriteService.UpdateListingQueueEntryAsync(requestedPriorityEntry) == null)
				{
					throw new InvalidOperationException($"Can't update entry {requestedQueueEntryId}");
				} 
				await _unitOfWork.CommitTransactionAsync();

				return requestedPriorityEntry.ToListingQueueEntryInfo();
			}
			catch
			{
				await _unitOfWork.RollBackTransactionAsync();
				throw;
			}
		}
	}
}
