using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
	internal class ListingItemRepository : IListingItemRepository
	{
		public Task<ListingItem?> AddListingItemAsync(ListingItem listingItem)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingItem>> AddListingItemsAsync(IEnumerable<ListingItem> listingItems)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteListingItem(Guid listingItemId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingItem?> GetListingItemByIdAsync(Guid listingItemId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingItem>> GetListingsItemsByListingIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}
	}
}
