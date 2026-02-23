namespace BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems
{
    public record CreateListingItemRequestDto(Guid CatalogId, string? Details)
    {
    }
}
