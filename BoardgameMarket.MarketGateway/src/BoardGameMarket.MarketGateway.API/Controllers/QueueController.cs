using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Enums;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.UseCases;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Users.Helpers;
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
		private readonly QueuesDataService _queuesDataService;
		private readonly UsersDataService _usersDataService;

		public QueueController(GetListingQueueUseCase getListingQueueUseCase, QueuesDataService queuesDataService, UsersDataService usersDataService)
		{
			_getListingQueueUseCase = getListingQueueUseCase;
			_queuesDataService = queuesDataService;
			_usersDataService = usersDataService;
		}

		[HttpGet()]
		public async Task<ActionResult<ListingQueueResponseDto>> GetQueue(Guid listingId)
		{
			ListingQueueResponseDto response = await _getListingQueueUseCase.ExecuteAsync(listingId);

			return Ok(response);
		}

		[HttpGet("subscription")]
		public async Task<ActionResult<ListingQueueEntryResponseDto>> GetSubscription(Guid listingId)
		{
			string? auth = Request.Headers.Authorization;

			if (!Guid.TryParse(auth, out var userId))
			{
				return BadRequest();
			}

			ListingQueueEntryInfo? queueEntry = await _queuesDataService.GetSubscriptionByUserAsync(listingId, userId);
			if (queueEntry == null)
			{
				return NoContent();
			}
			UserSummaryDto user = (await _usersDataService.GetUser(userId)).ToUserSummaryDto();
			return Ok(new ListingQueueEntryResponseDto(queueEntry.Id, queueEntry.Status, user, queueEntry.CreatedAt, queueEntry.ModifiedAt, null));
		}

		// TODO: validate subscription user
		[HttpPut("subscription")]
		public async Task<ActionResult<ListingQueueEntryResponseDto>> SubscribeToListing(Guid listingId)
		{
			if (!Guid.TryParse(Request.Headers.Authorization, out var userId))
			{
				return BadRequest();
			}

			ListingQueueEntryInfo queueEntry = await _queuesDataService.SubscribeToListingAsync(listingId, userId);
			UserSummaryDto user = (await _usersDataService.GetUser(userId)).ToUserSummaryDto();
			return Ok(new ListingQueueEntryResponseDto(queueEntry.Id, queueEntry.Status, user, queueEntry.CreatedAt, queueEntry.ModifiedAt, null));
		}

		// TODO: validate subscription user
		[HttpDelete("subscription/{subscriptionId:guid}")]
		public async Task<IActionResult> UnsubscribeFromListing(Guid listingId, Guid subscriptionId)
		{
			if (!Guid.TryParse(Request.Headers.Authorization, out var userId))
			{
				return BadRequest();
			}

			await _queuesDataService.UnsubscribeFromListingAsync(listingId, subscriptionId);

			return NoContent();
		}


	}
}
