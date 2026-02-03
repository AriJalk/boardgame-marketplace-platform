using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingGroupsController : ControllerBase
	{
		private readonly GetListingGroupDetailUseCase _getListingGroupDetailUseCase;
		private readonly GetListingGroupSummaryPageUseCase _getListingGroupSummaryPageUseCase;
		private readonly GetFilteredListingGroupSummariesUseCase _getFilteredListingGroupSummaries;
		private readonly QueuesDataService _queueService;

		public ListingGroupsController(GetListingGroupDetailUseCase getListingGroupDetailUseCase, GetListingGroupSummaryPageUseCase getListingGroupSummaryPageUseCase, GetFilteredListingGroupSummariesUseCase getFilteredListingGroupSummaries, QueuesDataService queueService)
		{
			_getListingGroupDetailUseCase = getListingGroupDetailUseCase;
			_getListingGroupSummaryPageUseCase = getListingGroupSummaryPageUseCase;
			_getFilteredListingGroupSummaries = getFilteredListingGroupSummaries;
			_queueService = queueService;
		}

		private async Task<ListingGroupSummaryDto[]> EnrichListingGroups(ListingGroupSummaryDto[] listingGroups)
		{
			string? auth = Request.Headers.Authorization;

			if (Guid.TryParse(auth, out var userId))
			{
				HashSet<Guid> subscribedListingIds = (await _queueService.GetSubscribedListingIdsAsync(userId)).ToHashSet();
				listingGroups = listingGroups.Select(lg => lg with
				{
					Listings = ListingEnricher.EnrichListingDtosWithSubscription(lg.Listings, subscribedListingIds)
				}).ToArray();
			}

			return listingGroups;
		}

		private async Task<ListingGroupDetailDto> EnrichListingGroup(ListingGroupDetailDto listingGroup)
		{
			string? auth = Request.Headers.Authorization;

			if (Guid.TryParse(auth, out var userId))
			{
				HashSet<Guid> subscribedListingIds = (await _queueService.GetSubscribedListingIdsAsync(userId)).ToHashSet();
				listingGroup = listingGroup with
				{
					Listings = ListingEnricher.EnrichListingDtosWithSubscription(listingGroup.Listings, subscribedListingIds)
				};
			}

			return listingGroup;
		}

		[HttpGet("{listingGroupId:guid}/detail")]
		public async Task<ActionResult<ListingGroupDetailDto>> GetListingGroupDetail(Guid listingGroupId)
		{
			ListingGroupDetailDto dto = await _getListingGroupDetailUseCase.Execute(listingGroupId);
			dto = await EnrichListingGroup(dto);
			return Ok(dto);
		}

		[HttpPost("page")]
		public async Task<ActionResult<PagedResult<ListingGroupSummaryDto>>> GetSummaryPage([FromBody] PagedListingGroupQuery query)
		{
			PagedResult<ListingGroupSummaryDto> page = await _getListingGroupSummaryPageUseCase.ExecuteAsync(query);

			page.Items = await EnrichListingGroups(page.Items);
			return Ok(page);
		}

		[HttpPost("filtered")]
		public async Task<ActionResult<ListingGroupSummaryDto[]>> GetFilteredListingGroupos([FromBody] ListingGroupFilter filter)
		{
			ListingGroupSummaryDto[] listingGroups = await _getFilteredListingGroupSummaries.ExecuteAsync(filter);
			listingGroups = await EnrichListingGroups(listingGroups);
			return Ok(listingGroups);
		}
	}
}
