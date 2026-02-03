using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;

namespace BoardGameMarket.MarketGateway.API.Application.Shared
{
	public static class ListingEnricher
	{
		public static ListingSummaryDto[] EnrichListingDtosWithSubscription(ListingSummaryDto[] listings, HashSet<Guid> subscribedListingIds)
		{
			ListingSummaryDto[] enrichedListings = listings.Select(l => l with
			{
				IsSubscribed = subscribedListingIds.Contains(l.Id)
			}).ToArray();

			return enrichedListings;
		}

		public static ListingDetailDto[] EnrichListingDtosWithSubscription(ListingDetailDto[] listings, HashSet<Guid> subscribedListingIds)
		{
			ListingDetailDto[] enrichedListings = listings.Select(l => l with
			{
				IsSubscribed = subscribedListingIds.Contains(l.Id)
			}).ToArray();

			return enrichedListings;
		}
	}
}
