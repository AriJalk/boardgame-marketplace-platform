using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SubscriptionsController : ControllerBase
	{
		private QueuesDataService _queueService;
		private readonly GetListingGroupSummaryPageUseCase _getListingGroupSummaryPageUseCase;

		public SubscriptionsController(QueuesDataService queueService, GetListingGroupSummaryPageUseCase getListingGroupSummaryPageUseCase)
		{
			_queueService = queueService;
			_getListingGroupSummaryPageUseCase = getListingGroupSummaryPageUseCase;
		}

		[HttpGet("listingGroups")]
		public async Task<ActionResult<PagedResult<ListingGroupSummaryDto>>> GetListingGroupsBySubscriber([FromQuery] int pageSize = 10, [FromQuery] string? cursor = null, [FromQuery] ListingGroupFilter? filter = null)
		{
			string? auth = Request.Headers.Authorization;

			if (!Guid.TryParse(auth, out var userId))
			{
				return BadRequest();
			}

			Guid[] listingIds = await _queueService.GetListingIdsBySubscribedUser(userId);
			filter ??= new ListingGroupFilter();
			filter.ListingIds = listingIds;

			PagedResult<ListingGroupSummaryDto> page = await _getListingGroupSummaryPageUseCase.ExecuteAsync(pageSize, cursor: cursor, filter: filter);
			return Ok(page);
		}
	}
}
