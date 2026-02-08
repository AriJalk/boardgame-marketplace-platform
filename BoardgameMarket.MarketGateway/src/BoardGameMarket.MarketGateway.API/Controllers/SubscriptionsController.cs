using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SubscriptionsController : ControllerBase
	{
		private readonly QueuesDataService _queueService;
		private readonly GetFilteredListingGroupSummariesUseCase _getFilteredListingGroupSummariesUseCase;

		public SubscriptionsController(QueuesDataService queueService, GetFilteredListingGroupSummariesUseCase getFilteredListingGroupSummariesUseCase)
		{
			_queueService = queueService;
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

			Guid[] subscribedListingIds = await _queueService.GetSubscribedListingIdsAsync(userId);
			HashSet<Guid> hashedSubscribedListingIds = subscribedListingIds.ToHashSet();

			ListingGroupFilter filter = new ListingGroupFilter()
			{
				ListingIds = subscribedListingIds,
			};

			ListingGroupSummaryDto[] listingGroups = await _getFilteredListingGroupSummariesUseCase.ExecuteAsync(filter);

			listingGroups = listingGroups.Select(lg =>
			{
				ListingSummaryDto[] enrichedListings = ListingEnricher.EnrichListingDtosWithSubscriptions(lg.Listings, hashedSubscribedListingIds);
				return (
				Group: lg with { Listings = enrichedListings },
				HasSubscribed: enrichedListings.Any(l => l.IsSubscribed == true && l.Status == ListingStatus.Active));
			})
				.Where(x => x.HasSubscribed)
				.Select(x => x.Group)
				.ToArray();

			return Ok(listingGroups);
		}
	}
}
