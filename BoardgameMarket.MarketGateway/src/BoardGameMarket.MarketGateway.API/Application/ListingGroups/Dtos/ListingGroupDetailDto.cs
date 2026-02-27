using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos
{
	public record ListingGroupDetailDto(Guid Id, UserSummaryDto Seller, DateTime PostingTime, string PickupOptions, string? Location, string? Details, ListingDetailDto[] Listings)
	{
	}

}
