using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
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
		private readonly LoginService _loginService;

		public ListingGroupsController(GetListingGroupDetailUseCase getListingGroupDetailUseCase, GetListingGroupSummaryPageUseCase getListingGroupSummaryPageUseCase, GetFilteredListingGroupSummariesUseCase getFilteredListingGroupSummaries, QueuesDataService queueService, LoginService loginService)
		{
			_getListingGroupDetailUseCase = getListingGroupDetailUseCase;
			_getListingGroupSummaryPageUseCase = getListingGroupSummaryPageUseCase;
			_getFilteredListingGroupSummaries = getFilteredListingGroupSummaries;
			_queueService = queueService;
			_loginService = loginService;
		}

		private async Task<ListingGroupSummaryDto[]> EnrichListingGroupSummariesWithIsSubscribed(ListingGroupSummaryDto[] listingGroups)
		{
			if (_loginService.LoggedInUser != null)
			{
				HashSet<Guid> subscribedListingIds = (await _queueService.GetSubscribedListingIdsAsync(_loginService.LoggedInUser.UserId)).ToHashSet();
				listingGroups = listingGroups.Select(lg => lg with
				{
					Listings = ListingEnricher.EnrichListingDtosWithSubscriptions(lg.Listings, subscribedListingIds)
				}).ToArray();
			}

			return listingGroups;
		}



		[HttpGet("{listingGroupId:guid}/detail")]
		public async Task<ActionResult<ListingGroupDetailDto>> GetListingGroupDetail(Guid listingGroupId)
		{

			ListingGroupDetailDto listingGroupDto = await _getListingGroupDetailUseCase.Execute(listingGroupId);

			if (_loginService.LoggedInUser != null)
			{
				ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
				{
					ListingIds = listingGroupDto.Listings.Select(l => l.Id).ToArray(),
					UserId = _loginService.LoggedInUser.UserId,
				};
				ListingQueueEntryInfo[] subscriptions = await _queueService.GetFilteredQueueEntries(filter);
				listingGroupDto = listingGroupDto with
				{
					Listings = ListingEnricher.EnrichListingDtosWithSubscriptions(listingGroupDto.Listings, subscriptions),
				};
			}

			return Ok(listingGroupDto);
		}

		[HttpPost("page")]
		public async Task<ActionResult<PagedResult<ListingGroupSummaryDto>>> GetSummaryPage([FromBody] PagedListingGroupQuery query)
		{
			PagedResult<ListingGroupSummaryDto> page = await _getListingGroupSummaryPageUseCase.ExecuteAsync(query);

			page.Items = await EnrichListingGroupSummariesWithIsSubscribed(page.Items);
			return Ok(page);
		}

		[HttpPost("filtered")]
		public async Task<ActionResult<ListingGroupSummaryDto[]>> GetFilteredListingGroups([FromBody] ListingGroupFilter filter)
		{
			ListingGroupSummaryDto[] listingGroups = await _getFilteredListingGroupSummaries.ExecuteAsync(filter);
			listingGroups = await EnrichListingGroupSummariesWithIsSubscribed(listingGroups);
			return Ok(listingGroups);
		}
	}
}
