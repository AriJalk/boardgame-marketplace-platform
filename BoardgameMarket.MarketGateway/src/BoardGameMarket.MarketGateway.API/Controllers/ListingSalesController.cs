using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingSales.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingSales.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingSales.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingSales.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
    [Route("api/listing-sales")]
    [ApiController]
    public class ListingSalesController : ControllerBase
    {
        private readonly ListingSalesDataService _listingSalesDataService;
        private readonly GetListingReservationResponseDtoUseCase _getListingReservationResponseDtoUseCase;

        public ListingSalesController(ListingSalesDataService listingSalesDataService, GetListingReservationResponseDtoUseCase getListingReservationResponseDtoUseCase)
        {
            _listingSalesDataService=listingSalesDataService;
            _getListingReservationResponseDtoUseCase=getListingReservationResponseDtoUseCase;
        }

        [HttpGet("{listingSaleId:guid}")]
        [Authorize]
        public async Task<ActionResult<ListingSaleResponseDto>> GetListingSale(Guid listingSaleId)
        {
            ListingSaleInfo listingSale = await _listingSalesDataService.GetListingSaleAsync(listingSaleId);

            ListingReservationResponseDto reservationDto = await _getListingReservationResponseDtoUseCase.ExecuteAsync(listingSale.ReservationId);

            return Ok(listingSale.ToListingSaleResponseDto(reservationDto));
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<ActionResult<ListingSaleResponseDto[]>> GetMyListingSales()
        {
            ListingSaleInfo[] listingSales = await _listingSalesDataService.GetUserListingSalesAsync();
            List<ListingSaleResponseDto> dtos = new List<ListingSaleResponseDto>();
            foreach (ListingSaleInfo listingSale in listingSales)
            {
                ListingReservationResponseDto reservation = await _getListingReservationResponseDtoUseCase.ExecuteAsync(listingSale.ReservationId);
                dtos.Add(listingSale.ToListingSaleResponseDto(reservation));
            }
            return Ok(dtos);
        }
    }
}
