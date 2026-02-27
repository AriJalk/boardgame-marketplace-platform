using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos
{
	public record ListingGroupSummaryDto(Guid Id, UserSummaryDto Seller, DateTime PostingTime, string PickupOptions, string? Location, string? Details, ListingSummaryDto[] Listings)
	{

	}

}
