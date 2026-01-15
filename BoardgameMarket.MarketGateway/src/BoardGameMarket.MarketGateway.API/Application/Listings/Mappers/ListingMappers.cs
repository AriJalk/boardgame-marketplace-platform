using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Mappers
{
	public static class ListingMappers
	{
		public static ListingSummaryDto ToListingSummaryDto(this ListingInGroupSummary listing)
		{
			return new ListingSummaryDto(listing.Id, listing.Title, listing.Price, listing.Details);
		}

		public static ListingDetailDto ToListingDetailDto(this ListingInGroupDetail listing, IEnumerable<ListingItemDto> listingItems)
		{
			return new ListingDetailDto(listing.Id, listing.Title, listing.Price, listing.Details, listingItems.ToArray());
		}

		public static ListingDetailDto ToListingDetailDto(this ListingDetail listing, IEnumerable<ListingItemDto> listingItems)
		{
			return new ListingDetailDto(listing.Id, listing.Title, listing.Price, listing.Details, listingItems.ToArray());
		}
	}
}
