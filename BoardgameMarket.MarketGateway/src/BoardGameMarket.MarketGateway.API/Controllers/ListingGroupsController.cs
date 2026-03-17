using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ListingGroupsDataService _listingGroupDataService;

        public ListingGroupsController(GetListingGroupDetailUseCase getListingGroupDetailUseCase, GetListingGroupSummaryPageUseCase getListingGroupSummaryPageUseCase, GetFilteredListingGroupSummariesUseCase getFilteredListingGroupSummariesUseCase, QueuesDataService queueDataService, LoginService loginService, ListingGroupsDataService listingGroupDataService)
        {
            _getListingGroupDetailUseCase = getListingGroupDetailUseCase;
            _getListingGroupSummaryPageUseCase = getListingGroupSummaryPageUseCase;
            _getFilteredListingGroupSummariesUseCase = getFilteredListingGroupSummariesUseCase;
            _queueDataService = queueDataService;
            _loginService = loginService;
            _listingGroupDataService = listingGroupDataService;
        }

        private async Task<ListingGroupSummaryDto[]> WithIsSubscribedAsync(ListingGroupSummaryDto[] listingGroups)
        {
            UserInfo? userInfo = await _loginService.GetCurrentUser();

            if (userInfo == null)
            {
                return listingGroups;
            }

            ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
            {
                UserId = userInfo.Id,
            };
            ListingQueueEntryInfo[] queueEntries = await _queueDataService.GetFilteredQueueEntriesAsync(filter);
            listingGroups = listingGroups.Select(lg => lg with
            {
                Listings = ListingEnricher.EnrichListingDtosWithIsSubscribed(lg.Listings, queueEntries.Select(qe => qe.ListingId).ToHashSet())
            }).ToArray();
            return listingGroups;
        }

        private async Task<ListingGroupDetailDto> WithSubscriptionAsync(ListingGroupDetailDto listingGroupDto)
        {
            UserInfo? user = await _loginService.GetCurrentUser();
            if (user == null || user.Id == listingGroupDto.Seller.UserId)
            {
                return listingGroupDto;
            }

            ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
            {
                ListingIds = listingGroupDto.Listings.Select(l => l.Id).ToArray(),
                UserId = user.Id,
            };

            ListingQueueEntryInfo[] subscriptions = await _queueDataService.GetFilteredQueueEntriesAsync(filter);
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
        [Authorize]
        public async Task<ActionResult<Guid>> CreateListingGroup(CreateListingGroupRequestDto request)
        {
            Guid requesterUserId = await _loginService.GetUserIdOrThrow();
            Guid listingGroupFromAddId = await _listingGroupDataService.AddListingGroup(request.ToCreateListingGroupRequest(requesterUserId));

            return Ok(listingGroupFromAddId);
        }

        [HttpPut("{listingGroupId}")]
        [Authorize]
        public async Task<ActionResult<Guid>> UpdateListingGroup(Guid listingGroupId, [FromBody] UpdateListingGroupRequestDto request)
        {
            if (listingGroupId != request.Id)
            {
                return BadRequest();
            }

            Guid listingGroupFromUpdateId = await _listingGroupDataService.UpdateListingGroup(request.ToUpdateListingGroupRequest());
            return Ok(listingGroupFromUpdateId);
        }
    }
}
