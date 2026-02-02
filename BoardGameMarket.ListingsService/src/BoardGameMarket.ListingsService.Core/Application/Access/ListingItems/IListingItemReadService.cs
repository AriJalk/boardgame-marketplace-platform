using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingItems
{
	public interface IListingItemReadService
	{
		Task<ListingItem?> GetListingItemById(Guid listingItemId);
		Task<IEnumerable<ListingItem>> GetListingItemsByListingIdAsync(Guid listingId);
		Task<IEnumerable<ListingItem>> GetListingItemsByListingIdsAsync(IEnumerable<Guid> listingIds);
		Task<IEnumerable<ListingItem>> GetListingItemsByCatalogIdAsync(Guid catalogId);
	}
}
