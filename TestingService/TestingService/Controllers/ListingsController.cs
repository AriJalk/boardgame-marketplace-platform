using Microsoft.AspNetCore.Mvc;
using TestingService.Domains.Listings.Services;
using TestingService.Domains.Queues.Dtos;

namespace TestingService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		QueuesDataService _queuesService;

		public ListingsController(QueuesDataService queuesService)
		{
			_queuesService = queuesService;
		}

		[HttpGet("{listingId:guid}/queues")]
		public IActionResult GetQueues(Guid listingId)
		{
			if (_queuesService.QueueEntries.TryGetValue(listingId, out var queueEntries))
			{
				QueuePageResponseDto pageResponse = new QueuePageResponseDto(
					TimeQueue: queueEntries.OrderBy(q => q.CreatedAt),
					PriorityQueue: queueEntries.Where(q => q.Priority != null).OrderBy(q => q.Priority));
				return Ok(pageResponse);
			}

			return NotFound();
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
