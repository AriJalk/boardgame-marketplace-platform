using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using System.Reflection;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingQueueEntries
{
	internal class ApproveListingQueueEntryUseCase
	{
		private readonly ListingFetcher _listingFetcher;
		private readonly ListingValidations _listingValidations;
		private readonly QueueEntryApprovalDomainService _queueEntryApprovalDomainService;
		private readonly IListingUpdaterService _listingUpdaterService;
		private readonly IListingQueueEntryUpdaterService _listingQueueEntryUpdaterService;
		private readonly IUnitOfWork _unitOfWork;

		private async Task UpdateListingAsync(Listing listing)
		{
			ListingUpdateRequest listingUpdateRequest = listing.ToUpdateRequest();
			listingUpdateRequest.Status = ListingStatus.Pending;
			if (await _listingUpdaterService.UpdateListingAsync(listingUpdateRequest) == null)
			{
				throw new InvalidOperationException("Can't update listing");
			}
		}

		private async Task UpdateListingQueueEntryAsync(ListingQueueEntry requestQueueEntry)
		{
			ListingQueueEntryUpdateRequest listingQueueEntryUpdateRequest = requestQueueEntry.ToUpdateRequest();
			listingQueueEntryUpdateRequest.Status = QueueStatus.Approved;
			if (await _listingQueueEntryUpdaterService.UpdateListingQueueEntryAsync(listingQueueEntryUpdateRequest) == null)
			{
				throw new InvalidOperationException("Can't update queue entry");
			}
		}

		private void ValidateRequest(ListingQueueEntry requestQueueEntry, Listing listing, Queue<ListingQueueEntry> listingQueueEntries, Queue<ListingQueuePriorityEntry> listingQueuePriorityEntries)
		{
			Result canApproveResult = _queueEntryApprovalDomainService.CanApprove(requestQueueEntry, listing, listingQueueEntries, listingQueuePriorityEntries);

			if (!canApproveResult.IsValid)
			{
				throw new InvalidOperationException(string.Join("\n", canApproveResult.Errors));
			}
		}


		public async Task<bool> ExecuteAsync(Guid listingQueueEntryId)
		{
			ListingQueueEntry? requestQueueEntry = await _listingFetcher.GetListingQueueEntryAsync(listingQueueEntryId);
			Guid listingId = requestQueueEntry.ListingId;
			Listing? listing = await _listingFetcher.GetListingAsync(listingId);
			await _listingValidations.ValidateListingNotSold(listing);


			Queue<ListingQueueEntry> listingQueueEntries = new Queue<ListingQueueEntry>(await _listingFetcher.GetListingQueueEntriesAsync(listingId));
			Queue<ListingQueuePriorityEntry> listingQueuePriorityEntries = new Queue<ListingQueuePriorityEntry>(await _listingFetcher.GetListingQueuePriorityEntries(listingQueueEntries));

			ValidateRequest(requestQueueEntry, listing, listingQueueEntries, listingQueuePriorityEntries);

			try
			{
				await _unitOfWork.BeginTransactionAsync();
				await UpdateListingAsync(listing);
				await UpdateListingQueueEntryAsync(requestQueueEntry);
				await _unitOfWork.CommitTransactionAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
