using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;

namespace BoardGameMarket.MarketGateway.API.Application.ListingReservations.ServiceContracts
{
    public record ListingReservationInfo(Guid Id, ListingSummary Listing, ListingQueueEntryInfo ListingQueueEntry, DateTime ReservedAt)
    {
    }
}
