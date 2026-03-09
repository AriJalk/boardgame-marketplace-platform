using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/queue-entries")]
	[ApiController]
	public class QueueEntriesController : ControllerBase
	{
		private readonly SubscriptionQueries _subscriptionQueries;
		private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
		private readonly ILogger<QueueEntriesController> _logger;

		public QueueEntriesController(SubscriptionQueries subscriptionQueries, IListingQueueEntryReadService listingQueueEntryReadService, ILogger<QueueEntriesController> logger)
		{
			_subscriptionQueries = subscriptionQueries;
			_listingQueueEntryReadService = listingQueueEntryReadService;
			_logger = logger;
		}

		[HttpPost("filter")]
		public async Task<ActionResult<ListingQueueEntryInfo[]>> GetFilteredQueueEntries([FromBody] ListingQueueEntryFilter filter)
		{
			if (filter.UserId == null &&
				filter.ListingIds?.Any() != true &&
				filter.ListingQueueEntryIds?.Any() != true)
			{
				throw new ValidationException("Filter must restrict results.");
			}
			IEnumerable<ListingQueueEntry> queueEntries = await _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter);

			return Ok(queueEntries.Select(qe => qe.ToListingQueueEntryInfo()));
		}
	}
}
