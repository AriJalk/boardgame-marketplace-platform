using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingItems
{
	public interface IListingItemWriteService
	{
		Task AddListingItemAsync(ListingItem listingItemToAdd);
		Task AddListingItemsAsync(IEnumerable<ListingItem> listingItemsToAdd);

		Task DeleteListingItemAsync(Guid listingItemId);
		Task DeleteListingItemsByListingIdAsync(Guid listingId);
		Task UpdateListingItemAsync(ListingItem listingItem);
	}
}
