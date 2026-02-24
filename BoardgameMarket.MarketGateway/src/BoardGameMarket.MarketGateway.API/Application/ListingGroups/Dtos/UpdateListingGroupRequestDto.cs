using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos
{
    public record UpdateListingGroupRequestDto(Guid Id,
        string? Details,
        string PickupOptions,
        string Location,
        UpdateListingRequestDto[] Listings)
    {
    }
}
