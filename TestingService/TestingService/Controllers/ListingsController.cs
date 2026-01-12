using Microsoft.AspNetCore.Mvc;
using TestingService.Domains.Queues.Dtos;
using TestingService.Domains.Queues.ServiceClients;
using TestingService.Domains.Queues.Services;
using TestingService.Domains.Users.Helpers;
using TestingService.Domains.Users.ServiceClients;
using TestingService.Domains.Users.Services;

namespace TestingService.Controllers
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
					TimeQueue: timeQueue.Select(q => BuildListingQueueEntryResponseDto(q)),
					PriorityQueue: priorityQueue.Select(q => BuildListingQueueEntryResponseDto(q)));
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
