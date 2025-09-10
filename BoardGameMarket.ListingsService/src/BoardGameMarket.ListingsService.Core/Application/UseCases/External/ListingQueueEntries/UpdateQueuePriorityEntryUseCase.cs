using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Services;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingQueueEntries
{
	internal class UpdateQueuePriorityEntryUseCase
	{
		private readonly ListingFetcher _listingFetcher;
		private readonly IListingQueuePriorityEntryUpdaterService _listingQueuePriorityEntryUpdaterService;
		private readonly PriorityQueueDomainService _priorityQueueDomainService;


		public async Task<ListingQueuePriorityEntry> ExecuteAsync(Guid listingQueueEntryId, decimal priorityWeight)
		{
			ListingQueueEntry listingQueueEntry = await _listingFetcher.GetListingQueueEntryAsync(listingQueueEntryId);
			IEnumerable<ListingQueuePriorityEntry> listingQueuePriorityEntries = await _listingFetcher.GetListingQueuePriorityEntires(listingQueueEntry.ListingId);
			_priorityQueueDomainService.ValidatePriorityEntry(listingQueueEntry, listingQueuePriorityEntries, priorityWeight, shouldExist: true);
			ListingQueuePriorityEntry? listingQueuePriorityEntry = await _listingQueuePriorityEntryUpdaterService.UpdateListingQueuePriorityEntryAsync(listingQueueEntryId, priorityWeight);
			if (listingQueuePriorityEntry == null)
			{
				throw new InvalidOperationException("Can't update priority entry");
			}
			return listingQueuePriorityEntry;
		}
	}
}
