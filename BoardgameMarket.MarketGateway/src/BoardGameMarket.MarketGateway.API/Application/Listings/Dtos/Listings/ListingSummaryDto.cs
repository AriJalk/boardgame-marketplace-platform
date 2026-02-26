using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings
{
	public record ListingSummaryDto(Guid Id, string? Title, decimal Price, string? Details, ListingStatus Status, bool? IsSubscribed, UserSummaryDto SellerDto)
	{
	}
}
