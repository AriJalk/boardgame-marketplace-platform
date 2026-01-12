using TestingService.Domains.Listings.Dtos;
using TestingService.Domains.Listings.ServiceClients.Listings;

namespace TestingService.Domains.Listings.Helpers
{
	public static class ListingMappers
	{
		public static ListingDetail ToListingDetail(this ListingInGroupDetail listingInGroupDetail, Guid listingGroupId)
		{
			return new ListingDetail(listingInGroupDetail.Id, listingInGroupDetail.Title, listingGroupId, listingInGroupDetail.Status, listingInGroupDetail.CreatedAt, listingInGroupDetail.ModifiedAt, listingInGroupDetail.Price, listingInGroupDetail.ListingItems, listingInGroupDetail.Details);
		}

		public static ListingSummaryDto ToListingSummaryDto(this ListingInGroupDetail listing)
		{
			return new ListingSummaryDto()
			{
				Id = listing.Id,
				Title = listing.Title,
				Price = listing.Price,
				Details = listing.Details
			};
		}

		public static ListingSummaryDto ToListingSummaryDto(this ListingDetail listing)
		{
			return new ListingSummaryDto()
			{
				Id = listing.Id,
				Title = listing.Title,
				Price = listing.Price,
				Details = listing.Details
			};
		}
	}
}
