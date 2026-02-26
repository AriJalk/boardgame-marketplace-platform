using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingReservations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly GetListingReservationUseCase _getListingReservationUseCase;
        private readonly GetListingReservationsUseCase _getListingResevationsUseCase;

        public ReservationsController(GetListingReservationUseCase getListingReservationUseCase, GetListingReservationsUseCase getListingResevationsUseCase)
        {
            _getListingReservationUseCase = getListingReservationUseCase;
            _getListingResevationsUseCase = getListingResevationsUseCase;
        }

        [HttpGet("{listingReservationId}")]
        public async Task<ActionResult<ListingReservationInfo>> GetListingReservation(Guid listingReservationId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
        {
            ListingReservationInfo reservation = await _getListingReservationUseCase.ExecuteAsync(listingReservationId, requesterUserId);
            return Ok(reservation);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<ListingReservationInfo>>> GetListingReservationsBySellerId(Guid userId)
        {
            IReadOnlyCollection<ListingReservationInfo> reservations = await _getListingResevationsUseCase.ExecuteAsync(userId);

            return Ok(reservations);
        }
    }
}
