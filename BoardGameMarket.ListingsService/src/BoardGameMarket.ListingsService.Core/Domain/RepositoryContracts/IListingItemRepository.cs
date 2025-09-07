using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingItemRepository
	{
		Task<ListingItem?> GetListingItemByIdAsync(Guid listingItemId);
		Task<IEnumerable<ListingItem>> GetListingsItemsByListingIdAsync(Guid listingId);
		Task<ListingItem?> AddListingItemAsync(ListingItem listingItem);
		Task<IEnumerable<ListingItem>> AddListingItemsAsync(IEnumerable<ListingItem> listingItems);
		Task<bool> DeleteListingItem(Guid listingItemId);
	}
}
