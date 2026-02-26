namespace BoardGameMarket.MarketGateway.API.Application.ListingReservations.ServiceContracts
{
    public record ListingReservationInfo(Guid Id, Guid ListingId, Guid ListingQueueEntryId, DateTime ReservedAt, Guid SellerId, Guid BuyerId)
    {
    }
}
