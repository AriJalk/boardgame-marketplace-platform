using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings
{
	public record ListingSummaryDto(Guid Id, string? Title, decimal Price, string? Details, ListingStatus Status, bool? IsSubscribed)
	{
	}
}
