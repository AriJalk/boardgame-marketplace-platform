using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingItems
{
	public interface IListingItemWriteService
	{
		Task<ListingItem?> AddListingItemAsync(ListingItem listingItemToAdd);
		Task<IEnumerable<ListingItem>> AddListingItemsAsync(IEnumerable<ListingItem> listingItemsToAdd);

		Task<bool> DeleteListingItemAsync(Guid listingItemId);
		Task DeleteListingItemsByListingIdAsync(Guid listingId);
	}
}
