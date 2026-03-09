using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingsSold.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingsSold.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Application.ListingsSold.Mappers
{
    public static class ListingSoldMappers
    {
        public static ListingSoldResponseDto ToListingSoldResponseDto(this ListingSoldInfo listingSold, ListingReservationResponseDto reservation)
        {
            return new ListingSoldResponseDto(listingSold.Id, listingSold.CompletedAt, reservation);
        }
    }
}
