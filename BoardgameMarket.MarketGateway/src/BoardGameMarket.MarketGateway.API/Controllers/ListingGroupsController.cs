using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/listing-groups")]
	[ApiController]
	public class ListingGroupsController : ControllerBase
	{
		private readonly GetListingGroupDetailUseCase _getListingGroupDetailUseCase;
		private readonly GetListingGroupSummaryPageUseCase _getListingGroupSummaryPageUseCase;
		private readonly GetFilteredListingGroupSummariesUseCase _getFilteredListingGroupSummariesUseCase;
		private readonly QueuesDataService _queueDataService;
		private readonly LoginService _loginService;
		private readonly AddListingGroupUseCase _addListingGroupUseCase;

        public ListingGroupsController(GetListingGroupDetailUseCase getListingGroupDetailUseCase, GetListingGroupSummaryPageUseCase getListingGroupSummaryPageUseCase, GetFilteredListingGroupSummariesUseCase getFilteredListingGroupSummariesUseCase, QueuesDataService queueDataService, LoginService loginService, AddListingGroupUseCase addListingGroupUseCase)
        {
            _getListingGroupDetailUseCase = getListingGroupDetailUseCase;
            _getListingGroupSummaryPageUseCase = getListingGroupSummaryPageUseCase;
            _getFilteredListingGroupSummariesUseCase = getFilteredListingGroupSummariesUseCase;
            _queueDataService = queueDataService;
            _loginService = loginService;
            _addListingGroupUseCase = addListingGroupUseCase;
        }

        private async Task<ListingGroupSummaryDto[]> WithIsSubscribedAsync(ListingGroupSummaryDto[] listingGroups)
		{
			if (_loginService.LoggedInUser != null)
			{
				HashSet<Guid> subscribedListingIds = (await _queueDataService.GetSubscribedListingIdsAsync(_loginService.LoggedInUser.UserId)).ToHashSet();
				listingGroups = listingGroups.Select(lg => lg with
				{
					Listings = ListingEnricher.EnrichListingDtosWithIsSubscribed(lg.Listings, subscribedListingIds)
				}).ToArray();
			}

			return listingGroups;
		}

		private async Task<ListingGroupDetailDto> WithSubscriptionAsync(ListingGroupDetailDto listingGroupDto)
		{
			UserInfo? user = _loginService.LoggedInUser;
			if (user == null || user.UserId == listingGroupDto.Seller.UserId)
			{
				return listingGroupDto;
			}

			ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
			{
				ListingIds = listingGroupDto.Listings.Select(l => l.Id).ToArray(),
				UserId = user.UserId,
			};

			ListingQueueEntryInfo[] subscriptions = await _queueDataService.GetFilteredQueueEntries(filter);
			return listingGroupDto with
			{
				Listings = ListingEnricher.EnrichListingDtosWithSubscriptions(listingGroupDto.Listings, subscriptions),
			};
		}



		[HttpGet("{listingGroupId:guid}/detail")]
		public async Task<ActionResult<ListingGroupDetailDto>> GetListingGroupDetail(Guid listingGroupId)
		{
			ListingGroupDetailDto listingGroupDto = await _getListingGroupDetailUseCase.Execute(listingGroupId);

			listingGroupDto = await WithSubscriptionAsync(listingGroupDto);

			return Ok(listingGroupDto);
		}

		[HttpPost("page")]
		public async Task<ActionResult<PagedResult<ListingGroupSummaryDto>>> GetSummaryPage([FromBody] PagedListingGroupQuery query)
		{
			PagedResult<ListingGroupSummaryDto> page = await _getListingGroupSummaryPageUseCase.ExecuteAsync(query);

			page.Items = await WithIsSubscribedAsync(page.Items);
			return Ok(page);
		}

		[HttpPost("filtered")]
		public async Task<ActionResult<ListingGroupSummaryDto[]>> GetFilteredListingGroups([FromBody] ListingGroupFilter filter)
		{
			ListingGroupSummaryDto[] listingGroups = await _getFilteredListingGroupSummariesUseCase.ExecuteAsync(filter);
			listingGroups = await WithIsSubscribedAsync(listingGroups);
			return Ok(listingGroups);
		}

		[HttpPost()]
		public async Task<ActionResult<ListingGroupDetailDto>> CreateListingGroup(CreateListingGroupRequestDto request)
		{
			if (_loginService.LoggedInUser == null)
			{
				return BadRequest();
			}
			ListingGroupDetailDto listingGroup = await _addListingGroupUseCase.ExecuteAsync(request.ToCreateListingGroupRequest(_loginService.LoggedInUser.UserId));

			return Ok(listingGroup);
		}
	}
}
