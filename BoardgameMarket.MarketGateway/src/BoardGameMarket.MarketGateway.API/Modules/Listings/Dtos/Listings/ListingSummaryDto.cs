namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.Listings
{
	public record ListingSummaryDto(Guid Id, string? Title, decimal Price, string? Details)
	{
	}
}
