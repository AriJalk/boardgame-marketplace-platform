using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/listings/{listingId:guid}/queue")]
	[ApiController]
	public class QueueController : ControllerBase
	{
		private readonly GetQueueUseCase _getQueueUseCase;
		private readonly AddListingQueueEntryUseCase _addListingQueueEntryUseCase;
		private readonly GetListingQueueEntryByIdUseCase _getListingQueueEntryByIdUseCase;
		private readonly AddListingQueueEntryPriorityUseCase _addListingQueueEntryPriorityUseCase;
		private readonly UpdateListingQueueEntryPriorityUseCase _updateListingQueueEntryPriorityUseCase;
		private readonly GetListingQueueEntryByUserIdUseCase _getListingQueueEntryByUserIdUseCase;
		private readonly DeleteListingQueueEntryUseCase _deleteListingQueueEntryUseCase;
		private readonly RemovePriorityFromQueueEntryUseCase _removePriorityFromQueueEntryUseCase;
		private ILogger<QueueController> _logger;

        public QueueController(GetQueueUseCase getQueueUseCase, AddListingQueueEntryUseCase addListingQueueEntryUseCase, GetListingQueueEntryByIdUseCase getListingQueueEntryByIdUseCase, AddListingQueueEntryPriorityUseCase addListingQueueEntryPriorityUseCase, UpdateListingQueueEntryPriorityUseCase updateListingQueueEntryPriorityUseCase, GetListingQueueEntryByUserIdUseCase getListingQueueEntryByUserIdUseCase, DeleteListingQueueEntryUseCase deleteListingQueueEntryUseCase, RemovePriorityFromQueueEntryUseCase removePriorityFromQueueEntryUseCase, ILogger<QueueController> logger)
        {
            _getQueueUseCase = getQueueUseCase;
            _addListingQueueEntryUseCase = addListingQueueEntryUseCase;
            _getListingQueueEntryByIdUseCase = getListingQueueEntryByIdUseCase;
            _addListingQueueEntryPriorityUseCase = addListingQueueEntryPriorityUseCase;
            _updateListingQueueEntryPriorityUseCase = updateListingQueueEntryPriorityUseCase;
            _getListingQueueEntryByUserIdUseCase = getListingQueueEntryByUserIdUseCase;
            _deleteListingQueueEntryUseCase = deleteListingQueueEntryUseCase;
            _removePriorityFromQueueEntryUseCase = removePriorityFromQueueEntryUseCase;
            _logger = logger;
        }

        [HttpGet()]
		public async Task<ActionResult<ListingQueueInfo>> GetQueue(Guid listingId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)

        {
			ListingQueueInfo queue = await _getQueueUseCase.Execute(listingId, requesterUserId);

			return Ok(queue);
		}

		[HttpGet("subscription/{listingQueueEntryId:guid}")]
		public async Task<ActionResult<ListingQueueEntryInfo>> GetListingQueueEntryById(Guid listingId, Guid listingQueueEntryId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
		{
			ListingQueueEntryInfo entry = await _getListingQueueEntryByIdUseCase.ExecuteAsync(listingId, listingQueueEntryId, requesterUserId);
			return Ok(entry);
		}

		[HttpGet("subscription/user/{userId:guid}")]
		public async Task<ActionResult<ListingQueueEntryInfo>> GetListingQueueEntryByUserId(Guid listingId, Guid userId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
		{
			ListingQueueEntryInfo? queueEntry = await _getListingQueueEntryByUserIdUseCase.ExecuteAsync(listingId, userId, requesterUserId);
			if (queueEntry == null)
			{
				return NoContent();
			}

			return Ok(queueEntry);
		}

		[HttpPost("subscription")]
		public async Task<ActionResult<ListingQueueEntryInfo>> AddListingQueueEntry(Guid listingId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
		{
			_logger.LogInformation($"{nameof(QueueController)}/{nameof(AddListingQueueEntry)}: ListingId: {listingId}, REQUESTER_USER_ID: {requesterUserId}");
			ListingQueueEntryInfo listingQueueEntry = await _addListingQueueEntryUseCase.ExecuteAsync(listingId, requesterUserId);
			return Ok(listingQueueEntry);
		}

		[HttpDelete("subscription/{listingQueueEntryId:guid}")]
		public async Task<IActionResult> DeleteListingQueueEntry(Guid listingId, Guid listingQueueEntryId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
		{
            _logger.LogInformation($"{nameof(QueueController)}/{nameof(DeleteListingQueueEntry)}: ListingId: {listingId}, REQUESTER_USER_ID: {requesterUserId}");
            await _deleteListingQueueEntryUseCase.ExecuteAsync(listingId, listingQueueEntryId, requesterUserId);
			return NoContent();
		}

		[HttpPost("subscription/{listingQueueEntryId:guid}/priority")]
		public async Task<ActionResult<ListingQueueEntryInfo>> AddPriorityToListingQueueEntry(Guid listingId, Guid listingQueueEntryId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
		{
			ListingQueueEntryInfo queueEntry = await _addListingQueueEntryPriorityUseCase.ExecuteAsync(listingId, listingQueueEntryId, requesterUserId);
			return Ok(queueEntry);
		}

		[HttpDelete("subscription/{listingQueueEntryId:guid}/priority")]
		public async Task<ActionResult<ListingQueueEntryInfo>> RemovePriorityFromEntry(Guid listingId, Guid listingQueueEntryId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
		{
			ListingQueueEntryInfo queueEntryUpdateResult = await _removePriorityFromQueueEntryUseCase.ExecuteAsync(listingId, listingQueueEntryId, requesterUserId);
			return Ok(queueEntryUpdateResult);
		}


        [HttpPatch("subscription/{listingQueueEntryId:guid}/priority")]
        public async Task<ActionResult<ListingQueueEntryInfo>> UpdateQueueEntryPriority(Guid listingId, Guid listingQueueEntryId, [FromBody] UpdateListingQueueEntryPriorityRequest updateRequest, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
        {
            ListingQueueEntryInfo queueEntry = await _updateListingQueueEntryPriorityUseCase.ExecuteAsync(listingId, listingQueueEntryId, updateRequest, requesterUserId);
            return Ok(queueEntry);
        }
    }
}
