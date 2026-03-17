using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.UseCases;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
    // TODO: buyer facing subscription dto is it's own type so no user calls + priority outside GetQueue from seller

    [Route("api/listings/{listingId:guid}/queue-entries")]
    [ApiController]
    public class ListingQueuesController : ControllerBase
    {
        private readonly GetListingQueueUseCase _getListingQueueUseCase;
        private readonly GetListingQueueEntryUseCase _getListingQueueEntryUseCase;
        private readonly QueuesDataService _queuesDataService;
        private readonly LoginService _loginService;

        public ListingQueuesController(GetListingQueueUseCase getListingQueueUseCase, GetListingQueueEntryUseCase getListingQueueEntryUseCase, QueuesDataService queuesDataService, LoginService loginService)
        {
            _getListingQueueUseCase = getListingQueueUseCase;
            _getListingQueueEntryUseCase = getListingQueueEntryUseCase;
            _queuesDataService = queuesDataService;
            _loginService = loginService;
        }


        #region Seller

        [HttpGet()]
        [Authorize]
        public async Task<ActionResult<ListingQueueResponseDto>> GetQueue(Guid listingId)
        {
            ListingQueueResponseDto response = await _getListingQueueUseCase.ExecuteAsync(listingId);

            return Ok(response);
        }


        [HttpPost("{listingQueueEntryId:guid}/priority")]
        [Authorize]
        public async Task<ActionResult<Guid>> AddPriority(Guid listingId, Guid listingQueueEntryId)
        {
            await _queuesDataService.AddPriorityToQueueEntry(listingId, listingQueueEntryId);
            return Ok(listingQueueEntryId);
        }


        [HttpPatch("{listingQueueEntryId:guid}/priority")]
        [Authorize]
        public async Task<IActionResult> UpdateListingQueueEntryPriority(Guid listingId, Guid listingQueueEntryId, [FromBody] UpdateListingQueueEntryPriorityRequestDto request)
        {

            await _queuesDataService.UpdateListingQueueEntryPriorityAsync(listingId, listingQueueEntryId, request);

            return NoContent();
        }

        [HttpDelete("{listingQueueEntryId:guid}/priority")]
        [Authorize]
        public async Task<ActionResult<Guid>> RemoveListingQueueEntryPriority(Guid listingId, Guid listingQueueEntryId)
        {
            await _queuesDataService.RemoveListingQueueEntryPriorityAsync(listingId, listingQueueEntryId);

            return Ok(listingQueueEntryId);
        }


        [HttpPost("{listingQueueEntryId:guid}/reserve")]
        [Authorize]
        public async Task<ActionResult<Guid>> ReserveListingToQueueEntry(Guid listingId, Guid listingQueueEntryId)
        {
            Guid reservationId = await _queuesDataService.ReserveListingToQueueEntry(listingId, listingQueueEntryId);
            return CreatedAtAction(
                actionName: nameof(ReservationsController.GetListingReservation),
                controllerName: "Reservations",
                routeValues: new { listingReservationId = reservationId },
                value: reservationId);
        }

        #endregion

        #region Buyer


        [HttpGet("my")]
        [Authorize]
        public async Task<ActionResult<ListingQueueEntryResponseDto>> GetMySubscription(Guid listingId)
        {

            Guid userId = await _loginService.GetUserIdOrThrow();
            ListingQueueEntryResponseDto? queueEntryResponse = await _getListingQueueEntryUseCase.ExecuteAsync(listingId, userId);

            if (queueEntryResponse == null)
            {
                return NoContent();
            }

            return Ok(queueEntryResponse);
        }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult<ListingSubscriptionResponseDto>> SubscribeToListing(Guid listingId)
        {
            ListingQueueEntryInfo queueEntry = await _queuesDataService.SubscribeToListingAsync(listingId);

            return Ok(queueEntry.ToListingSubscriptionResponseDto());
        }
        #endregion

        #region Shared

        [HttpDelete("{subscriptionId:guid}")]
        [Authorize]
        public async Task<IActionResult> UnsubscribeFromListing(Guid listingId, Guid subscriptionId)
        {
            await _queuesDataService.UnsubscribeFromListingAsync(listingId, subscriptionId);

            return NoContent();
        }
        #endregion

    }
}
