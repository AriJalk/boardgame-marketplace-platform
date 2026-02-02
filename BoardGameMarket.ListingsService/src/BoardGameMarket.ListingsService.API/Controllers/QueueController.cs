using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
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

		public QueueController(GetQueueUseCase getQueueUseCase, IListingQueueEntryReadService listingQueueReadService, IListingQueueEntryWriteService listingQueueWriteService, AddListingQueueEntryUseCase addListingQueueEntryUseCase)
		{
			_getQueueUseCase = getQueueUseCase;
			_listingQueueReadService = listingQueueReadService;
			_listingQueueWriteService = listingQueueWriteService;
			_addListingQueueEntryUseCase = addListingQueueEntryUseCase;
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
			ListingQueueEntry? queueEntry = await _listingQueueReadService.GetListingQueueEntryByListingIdAndUserId(listingId, userId);
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
	}
}
