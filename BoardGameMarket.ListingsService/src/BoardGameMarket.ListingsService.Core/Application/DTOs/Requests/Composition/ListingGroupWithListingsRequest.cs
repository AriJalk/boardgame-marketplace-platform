using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition
{
	internal class ListingGroupWithListingsRequest
	{
		public ListingGroupAddRequest ListingGroupAddRequest { get; set; } = new ListingGroupAddRequest();
		public IEnumerable<ListingWithListingItemsRequest> ListingWithListingItemsRequests { get; set; } = new List<ListingWithListingItemsRequest>();
	}
}
