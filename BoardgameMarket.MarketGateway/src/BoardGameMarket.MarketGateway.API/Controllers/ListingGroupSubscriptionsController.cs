using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
    [Route("api/listing-groups")]
    [ApiController]
    public class ListingGroupSubscriptionsController : ControllerBase
    {
        private readonly QueuesDataService _queueService;
        private readonly GetFilteredListingGroupSummariesUseCase _getFilteredListingGroupSummariesUseCase;
        private readonly LoginService _loginService;

        public ListingGroupSubscriptionsController(QueuesDataService queueService, GetFilteredListingGroupSummariesUseCase getFilteredListingGroupSummariesUseCase, LoginService loginService)
        {
            _queueService = queueService;
            _getFilteredListingGroupSummariesUseCase = getFilteredListingGroupSummariesUseCase;
            _loginService = loginService;
        }

        private async Task<ListingGroupSummaryDto[]> GetFilteredListingGroupsWithSubscription(Guid subscriberUserId, Guid? SellerUserId = null)
        {
            ListingQueueEntryFilter queueEntryFilter = new ListingQueueEntryFilter()
            {
                UserId = subscriberUserId,
            };
            ListingQueueEntryInfo[] queueEntries = await _queueService.GetFilteredQueueEntriesAsync(queueEntryFilter);
            Guid[] subscribedListingIds = queueEntries.Select(qe => qe.ListingId).ToArray();

            

            ListingGroupFilter filter = new ListingGroupFilter()
            {
                ListingIds = subscribedListingIds,
                SellerUserId = SellerUserId ?? null,
            };


            ListingGroupSummaryDto[] listingGroups = await _getFilteredListingGroupSummariesUseCase.ExecuteAsync(filter);

            listingGroups = listingGroups.Select(lg =>
            {
                ListingSummaryDto[] enrichedListings = ListingEnricher.EnrichListingDtosWithIsSubscribed(lg.Listings, subscribedListingIds.ToHashSet());
                return (
                Group: lg with { Listings = enrichedListings },
                HasSubscribed: enrichedListings.Any(l => l.IsSubscribed == true && (l.Status == ListingStatus.Active || l.Status == ListingStatus.Reserved)));
            })
                .Where(x => x.HasSubscribed)
                .Select(x => x.Group)
                .ToArray();

            return listingGroups;
        }


        [HttpGet("subscriptions")]
        [Authorize]
        public async Task<ActionResult<ListingGroupSummaryDto[]>> GetListingGroupsByLoggedUserSubscriptions()
        {
            if (_loginService.LoggedInUser == null)
            {
                return BadRequest();
            }

            ListingGroupSummaryDto[] listingGroups = await GetFilteredListingGroupsWithSubscription(_loginService.LoggedInUser.Id);

            return Ok(listingGroups);

        }


        [HttpGet("owned/subscribers/{subscriberUserId:guid}")]
        [Authorize]
        public async Task<ActionResult<ListingGroupSummaryDto[]>> GetLoggedUserListingGroupsBySubscriber(Guid subscriberUserId)
        {
            if (_loginService.LoggedInUser == null)
            {
                return BadRequest();
            }

            ListingGroupSummaryDto[] listingGroups = await GetFilteredListingGroupsWithSubscription(subscriberUserId, _loginService.LoggedInUser.Id);

            return Ok(listingGroups);
        }
    }
}
