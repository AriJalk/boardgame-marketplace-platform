namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients
{
    public record ListingGroupMetadataInfo(
        Guid Id,
        Guid SellerUserId,
        DateTime CreatedAt,
        DateTime ModifiedAt,
        string? Details,
        string PickupOptions,
        string Location)
    { }
}
