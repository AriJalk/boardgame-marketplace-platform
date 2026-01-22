using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
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
		private IListingQueueEntryGetterService _queueGetterService;
		private IListingQueueEntryAdderService _queueAdderService;
		private AddListingQueueEntryUseCase _addListingQueueEntryUseCase;

		public QueueController(GetQueueUseCase getQueueUseCase, IListingQueueEntryGetterService queueGetterService, IListingQueueEntryAdderService queueAdderService, AddListingQueueEntryUseCase addListingQueueEntryUseCase)
		{
			_getQueueUseCase = getQueueUseCase;
			_queueGetterService = queueGetterService;
			_queueAdderService = queueAdderService;
			_addListingQueueEntryUseCase = addListingQueueEntryUseCase;
		}

		[HttpGet()]
		public async Task<ActionResult<ListingQueueInfo>> GetQueue(Guid listingId)
		{
			ListingQueueInfo queue = await _getQueueUseCase.Execute(listingId);

			return Ok(queue);
		}

		[HttpGet("subscription")]
		public async Task<ActionResult<ListingQueueEntryInfo>> GetSubscription(Guid listingId, Guid userId)
		{
			ListingQueueEntry? queueEntry = await _queueGetterService.GetListingQueueEntryByListingIdAndUserId(listingId, userId);
			if (queueEntry == null)
			{
				return NoContent();
			}
			return Ok(queueEntry.ToListingQueueEntryInfo());
		}

		[HttpPost("subscription")]
		public async Task<ActionResult<ListingQueueEntryInfo>> AddListingQueueEntry(Guid listingId, AddListingQueueEntryRequest request)
		{
			ListingQueueEntryInfo listingQueueEntry = await _addListingQueueEntryUseCase.ExecuteAsync(listingId, request);
			return Ok(listingQueueEntry);
		}
	}
}
