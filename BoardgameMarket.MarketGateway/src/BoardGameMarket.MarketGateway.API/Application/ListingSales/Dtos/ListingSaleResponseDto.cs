using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingSales.Dtos
{
    public record ListingSaleResponseDto(Guid Id,
        DateTime CompletedAt,
        ListingReservationResponseDto Reservation)
    {

    }
}
