using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/subscriptions")]
	[ApiController]
	public class SubscriptionsController : ControllerBase
	{
		private readonly SubscriptionQueries _subscriptionQueries;
		private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
		private readonly ILogger<SubscriptionsController> _logger;

		public SubscriptionsController(SubscriptionQueries subscriptionQueries, IListingQueueEntryReadService listingQueueEntryReadService, ILogger<SubscriptionsController> logger)
		{
			_subscriptionQueries = subscriptionQueries;
			_listingQueueEntryReadService = listingQueueEntryReadService;
			_logger = logger;
		}

		[HttpGet("users/{userId:guid}/listings")]
		public async Task<ActionResult<Guid[]>> GetSubscribedListingIds(Guid userId)
		{
			//_logger.LogInformation($"{nameof(SubscriptionsController)}/{nameof(GetSubscribedListingIds)}: user - {userId}");
			IReadOnlyCollection<Guid> listingIds = await _subscriptionQueries.GetListingsBySubscribedUser(userId);

			return Ok(listingIds);
		}

		[HttpPost("query")]
		public async Task<ActionResult<ListingQueueEntryInfo[]>> GetFilteredQueueEntries([FromBody] ListingQueueEntryFilter filter)
		{
			IEnumerable<ListingQueueEntry> queueEntries = await _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter);

			return Ok(queueEntries.Select(qe => qe.ToListingQueueEntryInfo()));
		}
	}
}
