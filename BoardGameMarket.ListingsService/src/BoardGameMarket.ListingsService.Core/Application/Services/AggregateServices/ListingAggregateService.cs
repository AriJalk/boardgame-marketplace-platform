using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Services.AggregateServices
{
	public class ListingAggregateService
	{
		private IListingGetterService _listingGetterService;
		private IListingItemGetterService _listingItemGetterService;
		private IListingQueueEntryGetterService _listingQueueEntryGetterService;

		public ListingAggregateService(IListingGetterService listingGetterService, IListingItemGetterService listingItemGetterService, IListingQueueEntryGetterService listingQueueEntryGetterService)
		{
			_listingGetterService = listingGetterService;
			_listingItemGetterService = listingItemGetterService;
			_listingQueueEntryGetterService = listingQueueEntryGetterService;
		}

		async Task<IReadOnlyCollection<ListingItem>> LoadItemsAsync(Guid listingId)
		{
			return (await _listingItemGetterService.GetListingItemsByListingIdAsync(listingId)).ToList().AsReadOnly();
		}

		async Task<IReadOnlyCollection<ListingQueueEntry>> LoadQueueAsync(Guid listingId) {
			return (await _listingQueueEntryGetterService.GetListingQueueEntriesByListingIdAsync(listingId)).ToList().AsReadOnly();
		}

		private async Task<ListingAggregate> BuildListingAggregateAsync(Listing listing, bool loadItems, bool loadQueue)
		{
			Task<IReadOnlyCollection<ListingItem>>? itemsTask = loadItems ? LoadItemsAsync(listing.Id) : null;

			Task<IReadOnlyCollection<ListingQueueEntry>>? queueTask = loadQueue ? LoadQueueAsync(listing.Id) : null;

			await Task.WhenAll(itemsTask ?? Task.CompletedTask, queueTask ?? Task.CompletedTask);

			return new ListingAggregate(listing, itemsTask?.Result, queueTask?.Result);
		}


		public async Task<ListingAggregate> GetListingAggregateAsync(Guid listingId, bool loadItems, bool loadQueue)
		{
			Listing? listing = await _listingGetterService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				throw new KeyNotFoundException();
			}

			return await BuildListingAggregateAsync(listing, loadItems, loadQueue);
		}

		public async Task<IEnumerable<ListingAggregate>> GetListingAggregatesByListingGroupIdAsync(Guid listingGroupId, bool loadItems, bool loadQueue)
		{
			List<ListingAggregate> aggregates = new List<ListingAggregate>();

			IReadOnlyCollection<Listing> listings = (await _listingGetterService.GetListingsByListingGroupIdAsync(listingGroupId)).ToList().AsReadOnly();

			foreach (Listing listing in listings)
			{
				aggregates.Add(await BuildListingAggregateAsync(listing, loadItems, loadQueue));
			}

			IEnumerable<Task<ListingAggregate>> tasks = listings.Select(listing => BuildListingAggregateAsync(listing, loadItems, loadQueue));

			return await Task.WhenAll(tasks);
		}
	}
}
