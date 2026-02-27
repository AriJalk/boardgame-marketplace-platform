using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;

namespace BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos
{
    public record ListingReservationResponseDto(Guid Id, ListingSummaryDto Listing, ListingQueueEntryResponseDto ListingQueueEntry, DateTime ReservedAt, ListingGroupMetadataDto ListingGroupMetadata)
    {
    }
}
