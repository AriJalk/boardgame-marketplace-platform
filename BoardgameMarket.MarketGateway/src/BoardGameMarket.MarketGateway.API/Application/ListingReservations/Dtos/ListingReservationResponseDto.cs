namespace BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos
{
    // TODO: Aggregated structure
    public record ListingReservationResponseDto(Guid Id, Guid ListingId, Guid ListingQueueEntryId, DateTime ReservedAt, Guid SellerId, Guid BuyerId)
    {
    }
}
