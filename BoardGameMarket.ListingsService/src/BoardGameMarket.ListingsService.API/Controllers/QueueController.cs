using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.UseCases;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/listings/{listingId:guid}/[controller]")]
	[ApiController]
	public class QueueController : ControllerBase
	{
		private GetQueueUseCase _getQueueUseCase;
		private IListingQueueEntryReadService _listingQueueReadService;
		private IListingQueueEntryWriteService _listingQueueWriteService;
		private AddListingQueueEntryUseCase _addListingQueueEntryUseCase;
		private AddListingQueueEntryPriorityUseCase _addListingQueueEntryPriorityUseCase;
		private UpdateListingQueueEntryPriorityUseCase _updateListingQueueEntryPriorityUseCase;

        public QueueController(GetQueueUseCase getQueueUseCase, IListingQueueEntryReadService listingQueueReadService, IListingQueueEntryWriteService listingQueueWriteService, AddListingQueueEntryUseCase addListingQueueEntryUseCase, AddListingQueueEntryPriorityUseCase addListingQueueEntryPriorityUseCase, UpdateListingQueueEntryPriorityUseCase updateListingQueueEntryPriorityUseCase)
        {
            _getQueueUseCase = getQueueUseCase;
            _listingQueueReadService = listingQueueReadService;
            _listingQueueWriteService = listingQueueWriteService;
            _addListingQueueEntryUseCase = addListingQueueEntryUseCase;
            _addListingQueueEntryPriorityUseCase = addListingQueueEntryPriorityUseCase;
            _updateListingQueueEntryPriorityUseCase = updateListingQueueEntryPriorityUseCase;
        }

        [HttpGet()]
		public async Task<ActionResult<ListingQueueInfo>> GetQueue(Guid listingId)
		{
			ListingQueueInfo queue = await _getQueueUseCase.Execute(listingId);

			return Ok(queue);
		}

		[HttpGet("subscription/{listingQueueEntryId:guid}")]
		public async Task<ActionResult<ListingQueueEntryInfo>> GetListingQueueEntryById(Guid listingQueueEntryId)
		{
			ListingQueueEntry? entry = await _listingQueueReadService.GetListingQueueEntryByIdAsync(listingQueueEntryId);
			if (entry == null)
			{
				return NotFound();
			}
			return Ok(entry);
		}

		[HttpGet("subscription/user/{userId:guid}")]
		public async Task<ActionResult<ListingQueueEntryInfo>> GetSubscriptionByUserId(Guid listingId, Guid userId)
		{
			ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
			{
				UserId = userId,
				ListingIds = new List<Guid> { listingId },
			};
			ListingQueueEntry? queueEntry = (await _listingQueueReadService.GetFilteredListingQueueEntriesAsync(filter)).FirstOrDefault();
			if (queueEntry == null)
			{
				return NoContent();
			}
			return Ok(queueEntry.ToListingQueueEntryInfo());
		}

		[HttpPost("subscription")]
		public async Task<ActionResult<ListingQueueEntryInfo>> AddListingQueueEntry(Guid listingId, [FromBody] AddListingQueueEntryRequest request)
		{
			ListingQueueEntryInfo listingQueueEntry = await _addListingQueueEntryUseCase.ExecuteAsync(listingId, request);
			return Ok(listingQueueEntry);
		}

		[HttpDelete("subscription/{listingQueueEntryId:guid}")]
		public async Task<IActionResult> DeleteListingQueueEntry(Guid listingQueueEntryId)
		{
			await _listingQueueWriteService.DeleteListingQueueEntryAsync(listingQueueEntryId);
			return NoContent();
		}

		[HttpPost("subscription/{listingQueueEntryId:guid}/priority")]
		public async Task<ActionResult<ListingQueueEntryInfo>> AddPriorityToListingQueueEntry(Guid listingId, Guid listingQueueEntryId)
		{
			ListingQueueEntryInfo queueEntry = await _addListingQueueEntryPriorityUseCase.ExecuteAsync(listingId, listingQueueEntryId);
			return Ok(queueEntry);
		}

		[HttpDelete("subscription/{listingQueueEntryId:guid}/priority")]
		public async Task<ActionResult> RemovePriorityFromEntry(Guid listingId, Guid listingQueueEntryId)
		{
			ListingQueueEntry? queueEntry = await _listingQueueReadService.GetListingQueueEntryByIdAsync(listingQueueEntryId);

			if (queueEntry == null)
			{
				return NoContent();
			}

			queueEntry.Priority = null;
			ListingQueueEntry? queueEntryResponse = await _listingQueueWriteService.UpdateListingQueueEntryAsync(queueEntry);

			if (queueEntryResponse == null || queueEntryResponse.Priority != null)
			{
				return BadRequest();
			}
			return NoContent();
		}


        [HttpPatch("subscription/{listingQueueEntryId:guid}/priority")]
        public async Task<ActionResult<ListingQueueEntryInfo>> UpdateQueueEntryPriority(Guid listingId, Guid listingQueueEntryId, [FromBody] UpdateListingQueueEntryPriorityRequest updateRequest)
        {
            ListingQueueEntryInfo queueEntry = await _updateListingQueueEntryPriorityUseCase.ExecuteAsync(listingId, listingQueueEntryId, updateRequest);
            return Ok(queueEntry);
        }
    }
}
