using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingItems
{
	internal class ListingItemGetterService : IListingItemGetterService
	{
		private readonly IListingItemRepository _listingItemRepository;
		private readonly RequestCache _requestCache;
		public ListingItemGetterService(IListingItemRepository listingItemRepository, RequestCache requestCache)
		{
			_listingItemRepository = listingItemRepository;
			_requestCache = requestCache;
		}

		private async Task<IEnumerable<ListingItem>> GetFromCacheOrDb(Guid key, 
			Func<Guid, Task<IEnumerable<ListingItem>>> fetchFromDb,
			Func<Guid, IReadOnlyList<ListingItem>> fetchFromCache)
		{
			IReadOnlyList<ListingItem> cached = fetchFromCache(key);
			if (cached.Count > 0)
			{
				return cached;
			}
			IEnumerable<ListingItem> items = await fetchFromDb(key);
			foreach (ListingItem item in items)
			{
				_requestCache.AddListingItem(item);
			}
			return items;
		}


		public async Task<ListingItem?> GetListingItemById(Guid listingItemId)
		{
			//_logger.LogInformation($"{nameof(ListingItemGetterService)}:{nameof(GetListingItemById)}- get listing item {listingItemId}");
			if (_requestCache.TryGetListingItemById(listingItemId, out var listingItem))
			{
				return listingItem;
			}
			listingItem = await _listingItemRepository.GetListingItemByIdAsync(listingItemId);
			if (listingItem != null)
			{
				_requestCache.AddListingItem(listingItem);
			}
			return listingItem;
		}

		public async Task<IEnumerable<ListingItem>> GetListingItemsByCatalogIdAsync(Guid catalogId)
		{
			//_logger.LogInformation($"{nameof(ListingItemGetterService)}:{nameof(GetListingItemsByCatalogIdAsync)}- get catalog listing item {catalogId}");
			return await GetFromCacheOrDb(catalogId, _listingItemRepository.GetListingItemsByCatalogIdAsync, _requestCache.GetListingItemsByCatalogId);
		}

		public async Task<IEnumerable<ListingItem>> GetListingItemsByListingIdAsync(Guid listingId)
		{
			//_logger.LogInformation($"{nameof(ListingItemGetterService)}:{nameof(GetListingItemsByListingIdAsync)}- get listing items by listing {listingId}");
			return await GetFromCacheOrDb(listingId, _listingItemRepository.GetListingsItemsByListingIdAsync, _requestCache.GetListingItemsByListingId);
		}

		public async Task<IEnumerable<ListingItem>> GetListingItemsByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			//_logger.LogInformation($"{nameof(ListingItemGetterService)}:{nameof(GetListingItemsByListingIdsAsync)}- get listing items by listings {listingIds}");
			List<ListingItem> listingItems = new List<ListingItem>();
			HashSet<Guid> missingIds = new HashSet<Guid>();
			foreach(Guid listingId in listingIds)
			{
				IReadOnlyList<ListingItem> listingItemsFromCache = _requestCache.GetListingItemsByListingId(listingId);
				if (listingItemsFromCache.Count > 0)
				{
					listingItems.AddRange(listingItemsFromCache);
				}
				else
				{
					missingIds.Add(listingId);
				}
			}
			if (missingIds.Count > 0)
			{
				IEnumerable<ListingItem> listingItemsFromDb = await _listingItemRepository.GetListingItemsByListingIdsAsync(missingIds);
				listingItems.AddRange(listingItemsFromDb);
				foreach (ListingItem listingItem in listingItemsFromDb)
				{
					_requestCache.AddListingItem(listingItem);
				}
			}
			return listingItems;
		}
	}
}
