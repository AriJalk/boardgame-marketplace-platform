using Microsoft.AspNetCore.Mvc;
using BoardGameMarket.MarketGateway.API.Modules.Queues.Services;
using BoardGameMarket.MarketGateway.API.Modules.Users.Helpers;
using BoardGameMarket.MarketGateway.API.Modules.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Modules.Users.Services;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.ListingQueues;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingQueueEntries;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		QueuesDataService _queuesService;
		UsersService _usersService;

		public ListingsController(QueuesDataService queuesService, UsersService usersService)
		{
			_queuesService = queuesService;
			_usersService = usersService;
		}

		[HttpGet("{listingId:guid}/queues")]
		public IActionResult GetQueues(Guid listingId)
		{
			if (_queuesService.ListingQueueEntries.TryGetValue(listingId, out var queueEntries))
			{
				IEnumerable<ListingQueueEntryInfo> timeQueue = queueEntries.OrderBy(q => q.CreatedAt);
				IEnumerable<ListingQueueEntryInfo> priorityQueue = queueEntries.Where(q => q.Priority != null).OrderBy(q => q.Priority);

				QueuePageResponseDto pageResponse = new QueuePageResponseDto(
					QueueByTime: timeQueue.Select(q => BuildListingQueueEntryResponseDto(q)),
					QueueByPriority: priorityQueue.Select(q => BuildListingQueueEntryResponseDto(q)));
				return Ok(pageResponse);
			}

			return NotFound();
		}

		private ListingQueueEntryResponseDto BuildListingQueueEntryResponseDto(ListingQueueEntryInfo listingQueueEntryInfo)
		{
			UserInfo buyer = _usersService.Users[listingQueueEntryInfo.BuyerUserId];
			return new ListingQueueEntryResponseDto()
			{
				Id = listingQueueEntryInfo.Id,
				Buyer = buyer.ToUserSummaryDto(),
				CreatedAt = listingQueueEntryInfo.CreatedAt,
				ModifiedAt = listingQueueEntryInfo.ModifiedAt,
				Status = listingQueueEntryInfo.Status,
				Priority = listingQueueEntryInfo.Priority,
			};
		}

		[HttpPost("{listingId:guid}/queues/{queueEntryId:guid}")]
		public IActionResult UpdateQueueEntryPriority(Guid listingId, Guid queueEntryId, [FromBody] UpdateQueuePriorityRequest updateRequest)
		{
			bool isSuccessful;
			if (updateRequest.RequestedIndex == -1)
			{
				isSuccessful = _queuesService.RemoveQueuePriority(listingId, queueEntryId);
			}
			else
			{
				isSuccessful = _queuesService.UpdateQueuePriority(listingId, queueEntryId, updateRequest.RequestedIndex);
			}
			if (!isSuccessful)
			{
				return BadRequest();
			}
			return NoContent();
		}
	}
}
