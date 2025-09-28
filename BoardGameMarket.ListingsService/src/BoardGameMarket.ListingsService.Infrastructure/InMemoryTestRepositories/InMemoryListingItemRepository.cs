using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories
{
	internal class InMemoryListingItemRepository : IListingItemRepository
	{
		private readonly List<ListingItem> _listingItems = new List<ListingItem>();

		public Task<ListingItem?> AddListingItemAsync(ListingItem listingItem)
		{
			_listingItems.Add(listingItem);
			return Task.FromResult(listingItem);
		}

		public Task<IEnumerable<ListingItem>> AddListingItemsAsync(IEnumerable<ListingItem> listingItems)
		{
			_listingItems.AddRange(listingItems);
			return Task.FromResult(listingItems);
		}


		public Task<bool> DeleteListingItemAsync(Guid listingItemId)
		{
			_listingItems.Remove(_listingItems.FirstOrDefault(l => l.Id == listingItemId));
			return Task.FromResult(true);
		}

		public Task DeleteListingItemsByListingIdAsync(Guid listingId)
		{
			int removed = _listingItems.RemoveAll(i => i.ListingId == listingId);
			return Task.CompletedTask;
		}


		public Task<ListingItem?> GetListingItemByIdAsync(Guid listingItemId)
		{
			return Task.FromResult(_listingItems.FirstOrDefault(i => i.Id == listingItemId));
		}


		public Task<IEnumerable<ListingItem>> GetListingItemsByCatalogIdAsync(Guid catalogId)
		{
			return Task.FromResult(_listingItems.Where(i => i.CatalogId == catalogId));
		}

		public Task<IEnumerable<ListingItem>> GetListingItemsByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			return Task.FromResult(_listingItems.Where(i => listingIds.Contains(i.ListingId)));
		}

		public Task<IEnumerable<ListingItem>> GetListingsItemsByListingIdAsync(Guid listingId)
		{
			return Task.FromResult(_listingItems.Where(i => i.ListingId == listingId));
		}
	}
}
