namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems
{
	internal class ListingItemAddRequest
	{
		public Guid ListingId { get; set; }
		public Guid CatalogId { get; set; }
	}
}
