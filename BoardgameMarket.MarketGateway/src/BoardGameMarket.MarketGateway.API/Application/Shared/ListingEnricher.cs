using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;

namespace BoardGameMarket.MarketGateway.API.Application.Shared
{
	public static class ListingEnricher
	{
		public static ListingSummaryDto[] EnrichListingDtosWithIsSubscribed(ListingSummaryDto[] listings, HashSet<Guid> subscribedListingIds)
		{
			ListingSummaryDto[] enrichedListings = listings.Select(l => l with
			{
				IsSubscribed = subscribedListingIds.Contains(l.Id)
			}).ToArray();

			return enrichedListings;
		}

		public static ListingDetailDto[] EnrichListingDtosWithSubscriptions(ListingDetailDto[] listings, IEnumerable<ListingQueueEntryInfo> subscriptions)
		{
			Dictionary<Guid, ListingQueueEntryInfo> subscriptionsDictionary = subscriptions.ToDictionary(qe => qe.ListingId);

			ListingDetailDto[] enrichedListings = listings.Select(l =>
			{
				subscriptionsDictionary.TryGetValue(l.Id, out var queueEntry);
				return l with
				{
					Subscription = queueEntry?.ToListingSubscriptionResponseDto(),
				};
			}).ToArray();

			return enrichedListings;
		}
	}
}
