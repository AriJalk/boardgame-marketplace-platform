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

		public QueueController(GetQueueUseCase getQueueUseCase, IListingQueueEntryReadService listingQueueReadService, IListingQueueEntryWriteService listingQueueWriteService, AddListingQueueEntryUseCase addListingQueueEntryUseCase, AddListingQueueEntryPriorityUseCase addListingQueueEntryPriorityUseCase)
		{
			_getQueueUseCase = getQueueUseCase;
			_listingQueueReadService = listingQueueReadService;
			_listingQueueWriteService = listingQueueWriteService;
			_addListingQueueEntryUseCase = addListingQueueEntryUseCase;
			_addListingQueueEntryPriorityUseCase = addListingQueueEntryPriorityUseCase;
		}

		[HttpGet()]
		public async Task<ActionResult<ListingQueueInfo>> GetQueue(Guid listingId)
		{
			ListingQueueInfo queue = await _getQueueUseCase.Execute(listingId);

			return Ok(queue);
		}

		[HttpGet("subscription")]
		public async Task<ActionResult<ListingQueueEntryInfo>> GetSubscription(Guid listingId, [FromQuery] Guid userId)
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

		[HttpDelete("subscription/{queueEntryId:guid}")]
		public async Task<IActionResult> DeleteListingQueueEntry(Guid queueEntryId)
		{
			await _listingQueueWriteService.DeleteListingQueueEntryAsync(queueEntryId);
			return NoContent();
		}

		[HttpPost("subscription/{queueEntryId:guid}/priority")]
		public async Task<ActionResult<ListingQueueEntryInfo>> AddPriorityToEntry(Guid listingId, Guid queueEntryId)
		{
			ListingQueueEntryInfo queueEntry = await _addListingQueueEntryPriorityUseCase.ExecuteAsync(listingId, queueEntryId);
			return Ok(queueEntry);
		}
	}
}
