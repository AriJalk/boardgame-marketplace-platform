using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos
{
    public record ListingGroupMetadataDto(
        Guid Id,
        UserSummaryDto Seller,
        DateTime CreatedAt,
        DateTime ModifiedAt,
        string? Details,
        string PickupOptions,
        string Location)
    {
    }
}
