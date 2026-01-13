using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	public static class ListingGroupMappers
	{
		public static ListingGroupDetail ToListingGroupDetail(this ListingGroupAggregate listingGroupAggregate)
		{
			ListingGroup listingGroup = listingGroupAggregate.ListingGroup;
			IReadOnlyCollection<ListingInGroupDetail> listingsInfo = listingGroupAggregate.Listings.Select(l => l.ToListingInGroupDetail()).ToList().AsReadOnly();
			return new ListingGroupDetail(
				listingGroup.Id, listingGroup.SellerUserId, listingGroup.CreatedAt, listingGroup.ModifiedAt, listingGroup.Details, listingGroup.PickupOptions, listingGroup.Location, listingsInfo);
		}

		public static ListingGroupSummary ToListingGroupSummary(this ListingGroupAggregate listingGroupAggregate)
		{
			ListingGroup listingGroup = listingGroupAggregate.ListingGroup;
			IReadOnlyCollection<ListingInGroupSummary> listingsInfo = listingGroupAggregate.Listings.Select(l => l.Listing.ToListingInGroupSummary()).ToList().AsReadOnly();
			return new ListingGroupSummary(listingGroup.Id, listingGroup.SellerUserId, listingGroup.CreatedAt, listingGroup.ModifiedAt, listingGroup.Details, listingGroup.PickupOptions, listingGroup.Location, listingsInfo);
		}
	}
}
