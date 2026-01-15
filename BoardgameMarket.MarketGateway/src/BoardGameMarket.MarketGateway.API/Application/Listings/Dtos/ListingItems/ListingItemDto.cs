using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems
{
	public record ListingItemDto(Guid Id, CatalogItemDto CatalogItem, string? Details)
	{
	}
}
