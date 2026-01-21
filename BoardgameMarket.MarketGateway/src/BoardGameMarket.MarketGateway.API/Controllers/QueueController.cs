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

			if (Guid.TryParse(auth, out var buyer))
			{
				ListingQueueEntryInfo? queueEntry = await _queuesDataService.GetSubscriptionAsync(listingId, buyer);
				if (queueEntry == null)
				{
					return NoContent();
				}
				// TODO: its own dto
				UserSummaryDto user = (await _usersDataService.GetUser(buyer)).ToUserSummaryDto();
				return Ok(new ListingQueueEntryResponseDto(queueEntry.Id, queueEntry.Status, user, queueEntry.CreatedAt, queueEntry.ModifiedAt, null));
			}
			return BadRequest();
		}

		[HttpPut("subscription")]
		public async Task<ActionResult<ListingQueueEntryResponseDto>> Subscribe(Guid listingId)
		{
			return Ok(new ListingQueueEntryResponseDto(Guid.NewGuid(), QueueStatus.Queued, new UserSummaryDto() { DisplayName = "X", UserId = Guid.NewGuid() }, DateTime.Now, DateTime.Now, null));
		}
	}
}
