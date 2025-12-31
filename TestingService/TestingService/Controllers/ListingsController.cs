

using Microsoft.AspNetCore.Mvc;
using TestingService.Core.Application.DTOs.Responses;
using TestingService.Dtos;
using TestingService.Services;

namespace TestingService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		ListingsService _listingsService;

		public ListingsController(ListingsService listingsService)
		{
			_listingsService = listingsService;
		}

		[HttpGet("{listingId:guid}/queues")]
		public IActionResult GetQueues(Guid listingId)
		{
			if (_listingsService.QueueEntries.TryGetValue(listingId, out var queueEntries))
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
				isSuccessful = _listingsService.RemoveQueuePriority(listingId, queueEntryId);
			}
			else
			{
				isSuccessful = _listingsService.UpdateQueuePriority(listingId, queueEntryId, updateRequest.RequestedIndex);
			}
			if (!isSuccessful)
			{
				return BadRequest();
			}
			return NoContent();
		}
	}
}
