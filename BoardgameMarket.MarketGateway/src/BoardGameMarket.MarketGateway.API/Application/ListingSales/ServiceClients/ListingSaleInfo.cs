namespace BoardGameMarket.MarketGateway.API.Application.ListingSales.ServiceClients
{
    public record ListingSaleInfo(
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
