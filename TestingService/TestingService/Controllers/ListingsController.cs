

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

		[HttpGet("{id:guid}/queues")]
		public IActionResult GetQueues(Guid id)
		{
			if (_listingsService.QueueEntries.TryGetValue(id, out var queueEntries))
			{
				QueuePageResponseDto pageResponse = new QueuePageResponseDto(queueEntries.OrderBy(q => q.CreatedAt), queueEntries.Where(q => q.Priority != null).OrderBy(q => q.Priority));
				return Ok(pageResponse);
			}

			return NotFound();
		}

		[HttpPost("{listingId:guid}/queues/{queueEntryId:guid}")]
		public IActionResult UpdateQueueEntryPriority(Guid listingId, Guid queueEntryId, [FromBody] UpdateQueuePriorityRequest updateRequest)
		{
			bool isSuccessful = _listingsService.UpdateQueuePriority(listingId, queueEntryId, updateRequest.RequestedIndex);

			if (!isSuccessful)
			{
				return BadRequest();
			}
			return NoContent();
		}
	}
}
