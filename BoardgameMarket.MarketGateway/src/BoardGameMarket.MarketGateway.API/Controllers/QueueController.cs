using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.UseCases;
using BoardGameMarket.MarketGateway.API.Application.Listings.Services;
using BoardGameMarket.MarketGateway.API.Application.Queries;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
    // TODO: buyer facing subscription dto is it's own type so no user calls + priority outside GetQueue from seller

    [Route("api/listings/{listingId:guid}/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly GetListingQueueUseCase _getListingQueueUseCase;
        private readonly GetListingQueueEntryUseCase _getListingQueueEntryUseCase;
        private readonly ListingQueueEntryDtoAssembler _listingQueueEntryDtoAssembler;
        private readonly QueuesDataService _queuesDataService;
        private readonly LoginService _loginService;
        private readonly ListingQueries _listingQueries;

        public QueueController(GetListingQueueUseCase getListingQueueUseCase, GetListingQueueEntryUseCase getListingQueueEntryUseCase, ListingQueueEntryDtoAssembler listingQueueEntryDtoAssembler, QueuesDataService queuesDataService, LoginService loginService, ListingQueries listingQueries)
        {
            _getListingQueueUseCase = getListingQueueUseCase;
            _getListingQueueEntryUseCase = getListingQueueEntryUseCase;
            _listingQueueEntryDtoAssembler = listingQueueEntryDtoAssembler;
            _queuesDataService = queuesDataService;
            _loginService = loginService;
            _listingQueries = listingQueries;
        }


        [HttpGet()]
        public async Task<ActionResult<ListingQueueResponseDto>> GetQueue(Guid listingId)
        {
            Guid sellerId = await _listingQueries.GetSellerUserIdByListingId(listingId);
            if (sellerId != _loginService.LoggedInUser?.UserId)
            {
                return Unauthorized();
            }
            ListingQueueResponseDto response = await _getListingQueueUseCase.ExecuteAsync(listingId);

            return Ok(response);
        }

        [HttpPost("{listingQueueEntryId:guid}/priority")]
        public async Task<ActionResult<ListingQueueEntryResponseDto>> AddPriority(Guid listingId, Guid listingQueueEntryId)
        {
            Guid sellerId = await _listingQueries.GetSellerUserIdByListingId(listingId);

            if (sellerId != _loginService?.LoggedInUser?.UserId)
            {
                return Unauthorized();
            }

            ListingQueueEntryInfo queueEntry = await _queuesDataService.AddPriorityToQueueEntry(listingId, listingQueueEntryId);
            return Ok(queueEntry);
        }


        [HttpGet("subscription")]
        public async Task<ActionResult<ListingQueueEntryResponseDto>> GetSubscription(Guid listingId)
        {
            if (_loginService.LoggedInUser == null)
            {
                return BadRequest("No user logged in");
            }

            ListingQueueEntryResponseDto? queueEntryResponse = await _getListingQueueEntryUseCase.ExecuteAsync(listingId, _loginService.LoggedInUser.UserId);

            if (queueEntryResponse == null)
            {
                return NoContent();
            }

            return Ok(queueEntryResponse);
        }

        [HttpPut("subscription")]
        public async Task<ActionResult<ListingQueueEntryResponseDto>> SubscribeToListing(Guid listingId)
        {
            if (_loginService.LoggedInUser == null)
            {
                return BadRequest("No user logged in");
            }

            ListingQueueEntryInfo queueEntry = await _queuesDataService.SubscribeToListingAsync(listingId, _loginService.LoggedInUser.UserId);
            ListingQueueEntryResponseDto response = await _listingQueueEntryDtoAssembler.BuildQueueEntryResponseDto(queueEntry);
            return Ok(response);
        }

        [HttpDelete("subscription/{subscriptionId:guid}")]
        public async Task<IActionResult> UnsubscribeFromListing(Guid listingId, Guid subscriptionId)
        {
            if (_loginService.LoggedInUser == null)
            {
                return BadRequest("User not logged in");
            }

            ListingQueueEntryInfo? queueEntry = await _queuesDataService.GetSubscriptionForListingAsync(listingId, subscriptionId);

            if (queueEntry == null)
            {
                return NoContent();
            }

            Guid sellerId = await _listingQueries.GetSellerUserIdByListingId(listingId);

            if (queueEntry.BuyerUserId != _loginService.LoggedInUser.UserId && _loginService.LoggedInUser.UserId != sellerId)
            {
                return Unauthorized();
            }

            await _queuesDataService.UnsubscribeFromListingAsync(listingId, subscriptionId);

            return NoContent();
        }


    }
}
