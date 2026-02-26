using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.Listings
{
    public record ListingSummary(
        Guid Id,
        string Title,
        Guid ListingGroupId,
        ListingStatus Status,
        DateTime CreatedAt,
        DateTime ModifiedAt,
        decimal Price,
        string? Details,
        Guid SellerId)
    { }
}
