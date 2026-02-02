using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
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
		private readonly GetFilteredListingGroupSummariesUseCase _getFilteredListingGroupSummariesUseCase;

		public SubscriptionsController(QueuesDataService queueService, GetListingGroupSummaryPageUseCase getListingGroupSummaryPageUseCase, GetFilteredListingGroupSummariesUseCase getFilteredListingGroupSummariesUseCase)
		{
			_queueService = queueService;
			_getListingGroupSummaryPageUseCase = getListingGroupSummaryPageUseCase;
			_getFilteredListingGroupSummariesUseCase = getFilteredListingGroupSummariesUseCase;
		}

		[HttpGet("listingGroups")]
		public async Task<ActionResult<ListingGroupSummaryDto[]>> GetListingGroupsBySubscriber()
		{
			string? auth = Request.Headers.Authorization;

			if (!Guid.TryParse(auth, out var userId))
			{
				return BadRequest();
			}

			Guid[] listingIds = await _queueService.GetSubscribedListingIds(userId);
			ListingGroupFilter filter = new ListingGroupFilter()
			{
				ListingIds = listingIds,
			};

			ListingGroupSummaryDto[] listingGroups = await _getFilteredListingGroupSummariesUseCase.ExecuteAsync(filter);

			return Ok(listingGroups);
		}
	}
}
