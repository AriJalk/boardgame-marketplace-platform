using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Aggregates
{
	public class ListingAggregateBuilder
	{
		private IListingReadService _listingReadService;
		private IListingItemReadService _listingItemReadService;
		private ListingQueueAggregateBuilder _listingQueueAggregateBuilder;

		public ListingAggregateBuilder(IListingReadService listingReadService, IListingItemReadService listingItemReadService, ListingQueueAggregateBuilder listingQueueAggregateBuilder)
		{
			_listingReadService = listingReadService;
			_listingItemReadService = listingItemReadService;
			_listingQueueAggregateBuilder = listingQueueAggregateBuilder;
		}

		async Task<IEnumerable<ListingItem>> LoadItemsAsync(Guid listingId)
		{
			return await _listingItemReadService.GetListingItemsByListingIdAsync(listingId);
		}

		async Task<ListingQueueAggregate> LoadQueueAsync(Guid listingId)
		{
			return (await _listingQueueAggregateBuilder.GetListingQueueAggregate(listingId));
		}

		private async Task<ListingAggregate> BuildListingAggregateAsync(Listing listing, bool loadItems, bool loadQueue)
		{
			Task<IEnumerable<ListingItem>>? itemsTask = loadItems ? LoadItemsAsync(listing.Id) : null;

			Task<ListingQueueAggregate>? queueTask = loadQueue ? LoadQueueAsync(listing.Id) : null;

			await Task.WhenAll(itemsTask ?? Task.CompletedTask, queueTask ?? Task.CompletedTask);

			return new ListingAggregate(listing, itemsTask?.Result.ToList().AsReadOnly(), queueTask?.Result);
		}


		public async Task<ListingAggregate> GetListingAggregateByIdAsync(Guid listingId, bool loadItems, bool loadQueue)
		{
			Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				throw new KeyNotFoundException($"Listing {listingId} not found");
			}

			return await BuildListingAggregateAsync(listing, loadItems, loadQueue);
		}

		public async Task<IEnumerable<ListingAggregate>> GetListingAggregatesByIdsAsync(IEnumerable<Guid> listingIds, bool loadItems, bool loadQueue)
		{
			IEnumerable<Listing> listings = await _listingReadService.GetListingsByIdsAsync(listingIds);
			List<ListingAggregate> results = new List<ListingAggregate>();

			int maxConcurrency = 10;
			using SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);

			IEnumerable<Task>? tasks = listings.Select(async listing =>
			{
				await semaphore.WaitAsync();
				try
				{
					ListingAggregate listingAggregate = await BuildListingAggregateAsync(listing, loadItems, loadQueue);
					lock (results)
					{
						results.Add(listingAggregate);
					}
				}
				finally
				{
					semaphore.Release();
				}
			});

			await Task.WhenAll(tasks);

			return results;

		}

		public async Task<IEnumerable<ListingAggregate>> GetListingAggregatesByListingGroupIdAsync(Guid listingGroupId, bool loadItems, bool loadQueue)
		{
			IEnumerable<Listing> listings = (await _listingReadService.GetListingsByListingGroupIdAsync(listingGroupId)).ToList().AsReadOnly();

			IEnumerable<Task<ListingAggregate>> tasks = listings.Select(listing => BuildListingAggregateAsync(listing, loadItems, loadQueue));

			return await Task.WhenAll(tasks);
		}
	}
}
