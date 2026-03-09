using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.ServiceContracts;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingsSold.ServiceClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ListingReservationDataService _listingReservationDataService;
        private readonly GetListingReservationResponseDtoUseCase _getListingReservationResponseDtoUseCase;
        private readonly GetCurrentUserReservationsUseCase _getCurrentUserReservationsUseCase;

        public ReservationsController(ListingReservationDataService listingReservationDataService, GetListingReservationResponseDtoUseCase getListingReservationResponseDtoUseCase, GetCurrentUserReservationsUseCase getCurrentUserReservationsUseCase)
        {
            _listingReservationDataService = listingReservationDataService;
            _getListingReservationResponseDtoUseCase = getListingReservationResponseDtoUseCase;
            _getCurrentUserReservationsUseCase = getCurrentUserReservationsUseCase;
        }


        [HttpGet("{listingReservationId:guid}")]
        public async Task<ActionResult<ListingReservationResponseDto>> GetListingReservation(Guid listingReservationId)
        {
            ListingReservationResponseDto reservation = await _getListingReservationResponseDtoUseCase.ExecuteAsync(listingReservationId);

            return Ok(reservation);
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<ListingReservationResponseDto>>> GetMyListingReservations()
        {
            IEnumerable<ListingReservationResponseDto> dtos = await _getCurrentUserReservationsUseCase.ExecuteAsync();
            return Ok(dtos);
        }

        [HttpPost("{listingReservationId:guid}/sell")]
        public async Task <ActionResult<Guid>> ApproveSellToReservation(Guid listingReservationId)
        {
            ListingSoldInfo listingSoldInfo = await _listingReservationDataService.ApproveSellToReservation(listingReservationId);

            return Ok(listingSoldInfo.Id);
        }

        [HttpPost("{listingReservationId:guid}/cancel")]
        public async Task<IActionResult> CancelReservation(Guid listingReservationId)
        {
            await _listingReservationDataService.CancelReservation(listingReservationId);

            return NoContent();
        }
    }
}
