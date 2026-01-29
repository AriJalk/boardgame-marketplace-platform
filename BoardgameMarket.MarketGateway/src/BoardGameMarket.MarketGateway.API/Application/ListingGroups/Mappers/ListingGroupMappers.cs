using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers
{
	public static class ListingGroupMappers
	{

		public static ListingGroupSummaryDto ToListingGroupSummaryDto(this ListingGroupSummary listingGroup, ListingSummaryDto[] listings, UserSummaryDto seller)
		{
			return new ListingGroupSummaryDto(listingGroup.Id, seller, listingGroup.CreatedAt, listingGroup.PickupOptions, listingGroup.Location, listingGroup.Details, listings);
	
		}

		public static ListingGroupDetailDto ToListingGroupDetailDto(this ListingGroupDetail listingGroup, ListingDetailDto[] listings, UserSummaryDto seller)
		{
			return new ListingGroupDetailDto(listingGroup.Id, seller, listingGroup.CreatedAt, listingGroup.PickupOptions, listingGroup.Location, listingGroup.Details, listings);
		}

	}
}
