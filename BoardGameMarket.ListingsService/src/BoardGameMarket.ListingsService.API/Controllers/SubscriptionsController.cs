using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SubscriptionsController : ControllerBase
	{
		private readonly SubscriptionQueries _subscriptionQueries;
		private readonly ILogger _logger;

		public SubscriptionsController(SubscriptionQueries subscriptionQueries, ILogger<SubscriptionsController> logger)
		{
			_subscriptionQueries = subscriptionQueries;
			_logger = logger;
		}

		[HttpGet("users/{userId:guid}/listings")]
		public async Task<ActionResult<Guid[]>> GetSubscribedListingIds(Guid userId)
		{
			_logger.LogInformation($"{nameof(SubscriptionsController)}/{nameof(GetSubscribedListingIds)}: user-{userId}");
			IReadOnlyCollection<Guid> listingIds = await _subscriptionQueries.GetListingsBySubscribedUser(userId);

			return Ok(listingIds);
		}
	}
}
