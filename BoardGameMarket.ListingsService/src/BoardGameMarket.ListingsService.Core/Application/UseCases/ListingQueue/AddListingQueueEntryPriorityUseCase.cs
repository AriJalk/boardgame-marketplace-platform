using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
	public class AddListingQueueEntryPriorityUseCase
	{
		private readonly IListingReadService _listingReadService;
		private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
		private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ListingOwnershipQuery _listingOwnershipQuery;

        public AddListingQueueEntryPriorityUseCase(IListingReadService listingReadService, IListingQueueEntryReadService listingQueueEntryReadService, IListingQueueEntryWriteService listingQueueEntryWriteService, IUnitOfWork unitOfWork, ListingOwnershipQuery listingOwnershipQuery)
        {
            _listingReadService = listingReadService;
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingQueueEntryWriteService = listingQueueEntryWriteService;
            _unitOfWork = unitOfWork;
            _listingOwnershipQuery = listingOwnershipQuery;
        }



        // NOTE: not concurrency safe, need to make atomic in DB
        public async Task ExecuteAsync(Guid listingId, Guid requestedQueueEntryId, Guid requesterUserId)
		{
			Guid sellerId = await _listingOwnershipQuery.GetSellerUserIdByListingIdAsync(listingId);
			if (sellerId != requesterUserId)
			{
				throw new UnauthorizedAccessException();
			}
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
				IEnumerable<ListingQueueEntry> queue = (await _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter));
				IEnumerable<ListingQueueEntry> priorityQueue = queue.Where(qe => qe.Priority != null).OrderByDescending(qe => qe.Priority);

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
					return;
				}

				ListingQueueEntry? lastPriorityEntry = priorityQueue.FirstOrDefault();

				// If no priorities exist in queue
				if (lastPriorityEntry == null)
				{
					requestedPriorityEntry.Priority = QueueDomainService.INITIAL_PRIORITY;
				}
				else
				{
					requestedPriorityEntry.Priority = lastPriorityEntry.Priority + QueueDomainService.PRIORITY_GAP;
				}

				await _listingQueueEntryWriteService.UpdateListingQueueEntryAsync(requestedPriorityEntry);

				await _unitOfWork.CommitTransactionAsync();
			}
			catch
			{
				await _unitOfWork.RollBackTransactionAsync();
				throw;
			}
		}
	}
}
