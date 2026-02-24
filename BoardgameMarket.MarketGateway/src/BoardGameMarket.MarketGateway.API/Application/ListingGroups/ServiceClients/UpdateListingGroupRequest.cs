using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients
{
    public record UpdateListingGroupRequest(Guid Id,
        string? Details,
        string PickupOptions,
        string Location,
        UpdateListingRequest[] Listings)
    {
    }
}
