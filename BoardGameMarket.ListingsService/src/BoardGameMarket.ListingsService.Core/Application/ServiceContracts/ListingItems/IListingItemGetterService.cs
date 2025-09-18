using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems
{
	public interface IListingItemGetterService
	{
		Task<ListingItem?> GetListingItemById(Guid listingItemId);
		Task<IEnumerable<ListingItem>> GetListingItemsByListingIdAsync(Guid listingId);
		Task<IEnumerable<ListingItem>> GetListingItemsByListingIdsAsync(IEnumerable<Guid> listingIds);
		Task<IEnumerable<ListingItem>> GetListingItemsByCatalogIdAsync(Guid catalogId);
	}
}
