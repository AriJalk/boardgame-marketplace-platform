using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings
{
	public record ListingDetailDto(Guid Id, string? Title, decimal Price, string? Details, ListingItemDto[] Items)
	{

	}
}
