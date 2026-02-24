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
		Task<ListingItem?> AddListingItemAsync(ListingItem listingItem);
		Task<IEnumerable<ListingItem>> AddListingItemsAsync(IEnumerable<ListingItem> listingItems);
		Task<ListingItem?> UpdateListingItemAsync(ListingItem listingItem);
		Task<IEnumerable<ListingItem>> UpdateListingItemsAsync(IEnumerable<ListingItem> listingItems);
		Task<bool> DeleteListingItemAsync(Guid listingItemId);
		Task DeleteListingItemsByListingIdAsync(Guid listingId);
	}
}
