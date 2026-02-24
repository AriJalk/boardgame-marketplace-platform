using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.Listings
{
    public record UpdateListingRequest(
        Guid? Id,
        string Title,
        decimal Price,
        UpdateListingItemRequest[] ListingItems,
        string? Details)
    { }
}
