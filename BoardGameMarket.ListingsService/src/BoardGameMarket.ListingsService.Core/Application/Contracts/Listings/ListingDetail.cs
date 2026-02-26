using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.Listings
{
    public record ListingDetail(
        Guid Id,
        string Title,
        Guid ListingGroupId,
        ListingStatus Status,
        DateTime CreatedAt,
        DateTime ModifiedAt,
        decimal Price,
        ListingItemInfo[] ListingItems,
        string? Details, Guid SellerId)
    { }
}
