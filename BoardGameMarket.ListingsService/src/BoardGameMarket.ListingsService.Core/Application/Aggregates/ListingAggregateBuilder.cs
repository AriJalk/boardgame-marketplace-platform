using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Domain.Aggregates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Aggregates
{
	public class ListingAggregateBuilder
	{
		private readonly IListingReadService _listingReadService;
		private readonly IListingItemReadService _listingItemReadService;
		private readonly ListingQueueAggregateBuilder _listingQueueAggregateBuilder;

		public ListingAggregateBuilder(IListingReadService listingReadService, IListingItemReadService listingItemReadService, ListingQueueAggregateBuilder listingQueueAggregateBuilder)
		{
			_listingReadService = listingReadService;
			_listingItemReadService = listingItemReadService;
			_listingQueueAggregateBuilder = listingQueueAggregateBuilder;
		}

		private async Task<IEnumerable<ListingItem>> LoadItemsAsync(Guid listingId)
		{
			return await _listingItemReadService.GetListingItemsByListingIdAsync(listingId);
		}

		private async Task<ListingQueueAggregate> LoadQueueAsync(Guid listingId)
		{
			return (await _listingQueueAggregateBuilder.GetListingQueueAggregate(listingId));
		}

		public async Task<ListingAggregate> BuildListingAggregateAsync(Listing listing, bool loadItems, bool loadQueue)
		{
			Task<IEnumerable<ListingItem>>? itemsTask = loadItems ? LoadItemsAsync(listing.Id) : null;

			Task<ListingQueueAggregate>? queueTask = loadQueue ? LoadQueueAsync(listing.Id) : null;

			await Task.WhenAll(itemsTask ?? Task.CompletedTask, queueTask ?? Task.CompletedTask);

			return new ListingAggregate(listing, itemsTask?.Result.ToList().AsReadOnly(), queueTask?.Result);
		}

		public async Task<IEnumerable<ListingAggregate>> GetListingAggregatesByListingGroupIdAsync(Guid listingGroupId, bool loadItems, bool loadQueue)
		{
			IEnumerable<Listing> listings = (await _listingReadService.GetListingsByListingGroupIdAsync(listingGroupId)).ToList().AsReadOnly();

			IEnumerable<Task<ListingAggregate>> tasks = listings.Select(listing => BuildListingAggregateAsync(listing, loadItems, loadQueue));

			return await Task.WhenAll(tasks);
		}
	}
}
