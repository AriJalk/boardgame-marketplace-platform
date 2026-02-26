using BoardGameMarket.MarketGateway.API.Application.ListingReservations.ServiceContracts;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ListingReservationDataService _listingReservationDataService;

        public ReservationsController(ListingReservationDataService listingReservationDataService)
        {
            _listingReservationDataService = listingReservationDataService;
        }

        // TODO: construct and return DTO
        [HttpGet("{listingReservationId:guid}")]
        public async Task<ActionResult<ListingReservationInfo>> GetListingReservation(Guid listingReservationId)
        {
            ListingReservationInfo reservation = await _listingReservationDataService.GetListingReservation(listingReservationId);

            return Ok(reservation);
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<ListingReservationInfo>>> GetMyListingReservations()
        {
            IEnumerable<ListingReservationInfo> reservations = await _listingReservationDataService.GetCurrentUserListingReservations();

            return Ok(reservations);
        }
    }
}
