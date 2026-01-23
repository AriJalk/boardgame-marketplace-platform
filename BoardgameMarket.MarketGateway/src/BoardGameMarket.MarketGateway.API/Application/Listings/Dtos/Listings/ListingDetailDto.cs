using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems;
using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings
{
	public record ListingDetailDto(Guid Id, string? Title, decimal Price, string? Details, ListingItemDto[] Items, ListingStatus Status)
	{

	}
}
