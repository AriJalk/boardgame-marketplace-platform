using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Mappers
{
	public static class ListingMappers
	{
		public static ListingSummaryDto ToListingSummaryDto(this ListingSummary listing)
		{
			return new ListingSummaryDto(listing.Id, listing.Title, listing.Price, listing.Details, listing.Status, null);
		}


		public static ListingDetailDto ToListingDetailDto(this ListingDetail listing, IEnumerable<ListingItemDto> listingItems)
		{
			return new ListingDetailDto(listing.Id, listing.Title, listing.Price, listing.Details, listingItems.ToArray(), listing.Status, null);
		}

		public static CreateListingRequest ToCreateListingRequest(this CreateListingRequestDto dto)
		{
			return new CreateListingRequest(dto.Title, dto.Price, dto.ListingItems.Select(li => li.ToCreateListingItemRequest()).ToArray(), dto.Details);
		}

		public static UpdateListingRequest ToUpdateListingRequest(this UpdateListingRequestDto dto)
		{
			return new UpdateListingRequest(dto.Id, dto.Title, dto.Price, dto.ListingItems.Select(li => li.ToUpdateListingItemRequest()).ToArray(), dto.Details);
		}
	}
}
