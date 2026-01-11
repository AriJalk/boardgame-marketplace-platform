using System.Runtime.CompilerServices;
using TestingService.Dtos.Listing;
using TestingService.Dtos.ListingGroup;

namespace TestingService.Helpers
{
	public static class Mappers
	{
		public static ListingSummaryDto ToListingSummary(this ListingDetailDto listing)
		{
			return new ListingSummaryDto() { Id = listing.Id, Details = listing.Details, Price = listing.Price, Title = listing.Title };
		}

		public static ListingGroupSummaryDto ToListingGroupSummary(this ListingGroupDetailDto listingGroup)
		{
			return new ListingGroupSummaryDto()
			{
				Id = listingGroup.Id,
				Details = listingGroup.Details,
				Location = listingGroup.Location,
				PickupOptions = listingGroup.PickupOptions,
				PostingTime = listingGroup.PostingTime,
				Seller = listingGroup.Seller,
				Listings = listingGroup.Listings.Select(l => l.ToListingSummary())
			};
		}
	}
}
