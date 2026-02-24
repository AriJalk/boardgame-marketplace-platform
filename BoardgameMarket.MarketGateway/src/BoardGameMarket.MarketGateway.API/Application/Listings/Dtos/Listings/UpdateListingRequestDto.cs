using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings
{
    public record UpdateListingRequestDto(
        Guid? Id,
        string Title,
        decimal Price,
        UpdateListingItemRequestDto[] ListingItems,
        string? Details)
    { }
}
