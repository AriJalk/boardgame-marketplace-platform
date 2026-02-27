using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos
{
    public record CreateListingGroupRequestDto(
    string? Details,
    string PickupOptions,
    string Location,
    CreateListingRequestDto[] Listings)
    {
    }
}
