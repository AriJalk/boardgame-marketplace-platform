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
		Task<IEnumerable<ListingItem>> GetListingItemsByListingIdsAsync(IEnumerable<Guid> listingIds);
		Task<IEnumerable<ListingItem>> GetListingItemsByCatalogIdAsync(Guid catalogId);
		Task<int> AddListingItemAsync(ListingItem listingItem);
		Task<int> AddListingItemsAsync(IEnumerable<ListingItem> listingItems);
		Task<int> UpdateListingItemAsync(ListingItem listingItem);
		Task<int> UpdateListingItemsAsync(IEnumerable<ListingItem> listingItems);
		Task<int> DeleteListingItemAsync(Guid listingItemId);
		Task<int> DeleteListingItemsByListingIdAsync(Guid listingId);
	}
}
