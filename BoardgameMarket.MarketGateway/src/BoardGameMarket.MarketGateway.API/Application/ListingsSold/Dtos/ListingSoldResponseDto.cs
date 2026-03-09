using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingsSold.Dtos
{
    public record ListingSoldResponseDto(Guid Id,
        DateTime CompletedAt,
        ListingReservationResponseDto Reservation)
    {

    }
}
