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

		public QueueController(GetQueueUseCase getQueueUseCase, IListingQueueEntryGetterService queueGetterService)
		{
			_getQueueUseCase = getQueueUseCase;
			_queueGetterService = queueGetterService;
		}

		[HttpGet()]
		public async Task<ActionResult<ListingQueueInfo>> GetQueue(Guid listingId)
		{
			ListingQueueInfo queue = await _getQueueUseCase.Execute(listingId);

			return Ok(queue);
		}

		[HttpGet("subscription")]
		public async Task<ActionResult<ListingQueueEntryInfo?>> GetSubscription(Guid listingId, Guid buyerId)
		{
			ListingQueueEntry? queueEntry = await _queueGetterService.GetListingQueueEntryByListingIdAndBuyerUserId(listingId, buyerId);
			if (queueEntry == null)
			{
				return NoContent();
			}
			return Ok(queueEntry.ToListingQueueEntryInfo());
		}
	}
}
