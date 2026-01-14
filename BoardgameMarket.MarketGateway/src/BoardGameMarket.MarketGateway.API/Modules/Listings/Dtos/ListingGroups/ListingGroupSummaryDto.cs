using BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings;
using BoardGameMarket.MarketGateway.API.Modules.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.ListingGroups
{
	public record ListingGroupSummaryDto(Guid Id, UserSummaryDto Seller, DateTime PostingTime, string? PickupOptions, string? Location, string? Details, IEnumerable<ListingSummaryDto> Listings)
	{

	}

}
