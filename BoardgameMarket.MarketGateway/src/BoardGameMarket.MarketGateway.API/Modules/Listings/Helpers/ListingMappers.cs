using BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Helpers
{
	public static class ListingMappers
	{
		public static ListingDetail ToListingDetail(this ListingInGroupDetail listingInGroupDetail, Guid listingGroupId)
		{
			return new ListingDetail(listingInGroupDetail.Id, listingInGroupDetail.Title, listingGroupId, listingInGroupDetail.Status, listingInGroupDetail.CreatedAt, listingInGroupDetail.ModifiedAt, listingInGroupDetail.Price, listingInGroupDetail.ListingItems, listingInGroupDetail.Details);
		}

	}
}
