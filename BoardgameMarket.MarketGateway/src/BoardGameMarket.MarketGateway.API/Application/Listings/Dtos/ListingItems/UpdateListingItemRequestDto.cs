namespace BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems
{
    public record UpdateListingItemRequestDto(Guid? Id, Guid CatalogId, string? Details)
    {
    }
}
