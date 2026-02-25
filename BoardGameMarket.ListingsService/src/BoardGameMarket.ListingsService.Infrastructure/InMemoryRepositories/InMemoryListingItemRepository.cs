using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories
{
	internal class InMemoryListingItemRepository : IListingItemRepository
	{
		private readonly List<ListingItem> _listingItems = new List<ListingItem>();

		public Task<int> AddListingItemAsync(ListingItem listingItem)
		{
			_listingItems.Add(listingItem);
			return Task.FromResult(1);
		}

		public Task<int> AddListingItemsAsync(IEnumerable<ListingItem> listingItems)
		{
			_listingItems.AddRange(listingItems);
			return Task.FromResult(listingItems.Count());
		}


		public Task<int> DeleteListingItemAsync(Guid listingItemId)
		{
			int index = _listingItems.FindIndex(li => li.Id == listingItemId);
			if (index == -1)
			{
				return Task.FromResult(0);
			}
			_listingItems.RemoveAt(index);
			return Task.FromResult(1);
		}

		public Task<int> DeleteListingItemsByListingIdAsync(Guid listingId)
		{
			int rows = _listingItems.RemoveAll(li => li.ListingId == listingId);
			return Task.FromResult(rows);
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

        public Task<int> UpdateListingItemAsync(ListingItem listingItem)
        {
			int index = _listingItems.FindIndex(li => li.Id == listingItem.Id);
			if (index == -1)
			{
				return Task.FromResult(0);
			}
			_listingItems[index] = listingItem;
			return Task.FromResult(1);
        }

        public Task<int> UpdateListingItemsAsync(IEnumerable<ListingItem> listingItems)
        {
			int rows = 0;
			foreach (ListingItem listingItem in listingItems)
			{
				int index = _listingItems.FindIndex(li => li.Id == listingItem.Id);
				if (index == -1)
					continue;
				_listingItems[index] = listingItem;
				rows++;
			}
			return Task.FromResult(rows);
        }
    }
}
