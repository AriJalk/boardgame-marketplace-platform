using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems
{
	internal interface IListingItemGetterService
	{
		Task<ListingItem?> GetListingItemById(Guid listingItemId);
		Task<IEnumerable<ListingItem>> GetListingItemsByListingIdAsync(Guid listingId);
	}
}
