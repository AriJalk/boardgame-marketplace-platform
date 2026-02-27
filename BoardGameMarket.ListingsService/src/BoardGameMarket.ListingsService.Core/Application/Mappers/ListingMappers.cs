using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	public static class ListingMappers
	{
		public static ListingDetail ToListingDetail(this ListingAggregate listingAggregate)
		{
			Listing listing = listingAggregate.Listing;
			ListingItemInfo[] listingItemsInfo = listingAggregate.Items.Select(i => i.ToListingItemInfo()).ToArray();
			return new ListingDetail(listing.Id, listing.Title, listing.ListingGroupId, listing.Status, listing.CreatedAt, listing.ModifiedAt, listing.Price, listingItemsInfo, listing.Details, listing.SellerUserId);
		}

		public static ListingSummary ToListingSummary(this Listing listing)
		{
			return new ListingSummary(listing.Id, listing.Title, listing.ListingGroupId, listing.Status, listing.CreatedAt, listing.ModifiedAt, listing.Price, listing.Details, listing.SellerUserId);
		}
	}
}
