using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingSales.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingSales.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Application.ListingSales.Mappers
{
    public static class ListingSaleMappers
    {
        public static ListingSaleResponseDto ToListingSaleResponseDto(this ListingSaleInfo listingSale, ListingReservationResponseDto reservation)
        {
            return new ListingSaleResponseDto(listingSale.Id, listingSale.CompletedAt, reservation);
        }
    }
}
