using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings
{
    public record CreateListingRequestDto(string Title,
    decimal Price,
    CreateListingItemRequestDto[] ListingItems,
    string? Details)
    {
    }
}
