using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingsSold.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingsSold.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingsSold.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingsSold.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
    [Route("api/listings-sold")]
    [ApiController]
    public class ListingsSoldController : ControllerBase
    {
        private readonly ListingsSoldDataService _listingsSoldDataService;
        private readonly GetListingReservationResponseDtoUseCase _getListingReservationResponseDtoUseCase;

        public ListingsSoldController(ListingsSoldDataService listingsSoldDataService, GetListingReservationResponseDtoUseCase getListingReservationResponseDtoUseCase)
        {
            _listingsSoldDataService=listingsSoldDataService;
            _getListingReservationResponseDtoUseCase=getListingReservationResponseDtoUseCase;
        }

        [HttpGet("{listingSoldId:guid}")]
        public async Task<ActionResult<ListingSoldResponseDto>> GetListingSold(Guid listingSoldId)
        {
            ListingSoldInfo listingSold = await _listingsSoldDataService.GetListingSoldAsync(listingSoldId);

            ListingReservationResponseDto reservationDto = await _getListingReservationResponseDtoUseCase.ExecuteAsync(listingSold.ReservationId);

            return Ok(listingSold.ToListingSoldResponseDto(reservationDto));
        }

        [HttpGet("my")]
        public async Task<ActionResult<ListingSoldResponseDto[]>> GetMyListingsSold()
        {
            ListingSoldInfo[] listingsSold = await _listingsSoldDataService.GetUserListingsSoldAsync();
            List<ListingSoldResponseDto> dtos = new List<ListingSoldResponseDto>();
            foreach (ListingSoldInfo listingSold in listingsSold)
            {
                ListingReservationResponseDto reservation = await _getListingReservationResponseDtoUseCase.ExecuteAsync(listingSold.ReservationId);
                dtos.Add(listingSold.ToListingSoldResponseDto(reservation));
            }
            return Ok(dtos);
        }
    }
}
