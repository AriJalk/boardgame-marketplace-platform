using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition
{
	internal class ListingWithListingItemsRequest
	{
		public ListingAddRequest ListingAddRequest { get; set; } = new ListingAddRequest();
		public IEnumerable<ListingItemAddRequest> ListingItemAddRequests { get; set; } = new List<ListingItemAddRequest>();
	}
}
