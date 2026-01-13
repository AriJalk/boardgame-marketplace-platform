using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	internal static class ListingItemMappers
	{
		public static ListingItemInfo ToListingItemInfo(this ListingItem listingItem)
		{
			return new ListingItemInfo(listingItem.Id, listingItem.CatalogId, listingItem.Details);
		}
	}
}
