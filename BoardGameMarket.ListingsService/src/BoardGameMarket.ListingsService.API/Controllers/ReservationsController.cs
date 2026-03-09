using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingSold;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingReservations;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
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
        private readonly ApproveReservationSellUseCase _approveReservationSellUseCase;
        private readonly CancelReservationUseCase _cancelReservationUseCase;

        public ReservationsController(GetListingReservationUseCase getListingReservationUseCase, GetListingReservationsUseCase getListingResevationsUseCase, ApproveReservationSellUseCase approveReservationSellUseCase, CancelReservationUseCase cancelReservationUseCase)
        {
            _getListingReservationUseCase=getListingReservationUseCase;
            _getListingResevationsUseCase=getListingResevationsUseCase;
            _approveReservationSellUseCase=approveReservationSellUseCase;
            _cancelReservationUseCase=cancelReservationUseCase;
        }

        [HttpGet("{listingReservationId:guid}")]
        public async Task<ActionResult<ListingReservationInfo>> GetListingReservation(Guid listingReservationId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
        {
            ListingReservationInfo reservation = await _getListingReservationUseCase.ExecuteAsync(listingReservationId, requesterUserId);
            return Ok(reservation);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<ListingReservationInfo>>> GetListingReservationsByUserId(Guid userId)
        {
            IReadOnlyCollection<ListingReservationInfo> reservations = await _getListingResevationsUseCase.ExecuteAsync(userId);

            return Ok(reservations);
        }

        [HttpPost("{listingReservationId:guid}/sell")]
        public async Task<ActionResult<ListingSoldInfo>> SellToReservation(Guid listingReservationId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
        {
            ListingSoldInfo listingSold = await _approveReservationSellUseCase.ExecuteAsync(listingReservationId, requesterUserId);
            return CreatedAtAction(nameof(GetListingReservation), new { listingReservationId = listingSold.Id }, listingSold);
        }

        [HttpPost("{listingReservationId:Guid}/cancel")]
        public async Task<ActionResult<ListingSoldInfo>> CancelReservation(Guid listingReservationId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
        {
            await _cancelReservationUseCase.ExecuteAsync(listingReservationId, requesterUserId);

            return NoContent();
        }
    }
}
