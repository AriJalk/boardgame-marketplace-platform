namespace BoardGameMarket.MarketGateway.API.Application.ListingsSold.ServiceClients
{
    public record ListingSoldInfo(
        Guid Id,
        Guid ListingId,
        DateTime CompletedAt,
        Guid ReservationId,
        Guid ListingQueueEntryId,
        Guid SellerUserId,
        Guid BuyerUserId
        )
    {
    }
}
