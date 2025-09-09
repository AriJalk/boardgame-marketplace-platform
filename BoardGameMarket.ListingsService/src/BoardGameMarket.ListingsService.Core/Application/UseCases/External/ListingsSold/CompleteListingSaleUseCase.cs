using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Core.Domain.Services;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingsSold
{
	internal class CompleteListingSaleUseCase
	{
		private readonly IListingSoldGetterService _listingSoldGetterService;
		private readonly IListingSoldAdderService _listingSoldAdderService;
		private readonly IListingQueueEntryGetterService _listingQueueEntryGetterService;
		private readonly IListingQueuePriorityEntryGetterService _listingQueuePriorityEntryGetterService;
		private readonly SaleDomainService _saleDomainService;
		private readonly IListingGetterService _listingGetterService;
		private readonly IListingUpdaterService _listingUpdaterService;
		private readonly IListingQueueEntryUpdaterService _listingQueueEntryUpdaterService;
		private readonly IUnitOfWork _unitOfWork;

		private async Task<ListingSold> AddListingSoldAsync(ListingSoldAddRequest listingSoldAddRequest)
		{
			ListingSold? listingSold = await _listingSoldAdderService.AddListingSoldAsync(listingSoldAddRequest);
			if (listingSold == null)
			{
				throw new InvalidOperationException("Can't add sale");
			}
			return listingSold;
		}

		private async Task UpdateListingAsync(Guid listingId)
		{
			Listing? listing = await _listingGetterService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				throw new KeyNotFoundException("Listing not found");
			}
			ListingUpdateRequest listingUpdateRequest = listing.ToUpdateRequest();
			listingUpdateRequest.Status = Domain.Enums.ListingStatus.Sold;
			if (await _listingUpdaterService.UpdateListingAsync(listingUpdateRequest) == null)
			{
				throw new InvalidOperationException("Can't update listing");
			}
		}

		private async Task UpdateListingQueueEntryAsync(ListingQueueEntry requestQueueEntry)
		{
			ListingQueueEntryUpdateRequest listingQueueEntryUpdateRequest = requestQueueEntry.ToUpdateRequest();
			listingQueueEntryUpdateRequest.Status = Domain.Enums.QueueStatus.Completed;
			if (await _listingQueueEntryUpdaterService.UpdateListingQueueEntryAsync(listingQueueEntryUpdateRequest) == null)
			{
				throw new InvalidOperationException("Can't update queue entry");
			}
		}

		private async Task<Queue<ListingQueueEntry>> GetListingQueueEntriesAsync(Guid listingId)
		{
			return await _listingQueueEntryGetterService.GetListingQueueEntriesByListingIdAsync(listingId);
		}

		private async Task<Queue<ListingQueuePriorityEntry>> GetListingQueuePriorityEntries(IEnumerable<ListingQueueEntry> listingQueueEntries)
		{
			return await _listingQueuePriorityEntryGetterService.GetListingQueuePriorityEntriesAsync(listingQueueEntries.Select(qe => qe.Id));
		}

		private void ValidateRequest(ListingQueueEntry requestQueueEntry, Queue<ListingQueueEntry> listingQueueEntries, Queue<ListingQueuePriorityEntry> listingQueuePriorityEntries)
		{
			Result canSellResult = _saleDomainService.CanSell(requestQueueEntry, listingQueueEntries, listingQueuePriorityEntries);

			if (!canSellResult.IsValid)
			{
				throw new InvalidOperationException(string.Join("\n", canSellResult.Errors));
			}
		}

		private async Task<ListingQueueEntry> GetListingQueueEntryAsync(Guid listingQueueEntryId)
		{
			ListingQueueEntry? listingQueueEntry = await _listingQueueEntryGetterService.GetListingQueueEntryByIdAsync(listingQueueEntryId);
			if (listingQueueEntry == null)
			{
				throw new KeyNotFoundException("Listing queue entry not found");
			}
			return listingQueueEntry;
		}

		private async Task ValidateListingNotSold(Guid listingId)
		{
			if (await _listingSoldGetterService.GetListingSoldByListingId(listingId) != null)
			{
				throw new InvalidOperationException("Listing already sold");
			}
		}

		public async Task<ListingSold> ExecuteAsync(ListingSoldAddRequest listingSoldAddRequest)
		{
			Guid listingId = listingSoldAddRequest.ListingId;
			await ValidateListingNotSold(listingId);
			ListingQueueEntry? requestQueueEntry = await GetListingQueueEntryAsync(listingSoldAddRequest.ListingQueueEntryId);

			Queue<ListingQueueEntry> listingQueueEntries = await GetListingQueueEntriesAsync(listingId);
			Queue<ListingQueuePriorityEntry> listingQueuePriorityEntries = await GetListingQueuePriorityEntries(listingQueueEntries);

			ValidateRequest(requestQueueEntry, listingQueueEntries, listingQueuePriorityEntries);

			try
			{
				await _unitOfWork.BeginTransactionAsync();
				ListingSold listingSold = await AddListingSoldAsync(listingSoldAddRequest);
				await UpdateListingAsync(listingId);
				await UpdateListingQueueEntryAsync(requestQueueEntry);
				await _unitOfWork.CommitTransactionAsync();
				// TODO: return result
				return listingSold;
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
