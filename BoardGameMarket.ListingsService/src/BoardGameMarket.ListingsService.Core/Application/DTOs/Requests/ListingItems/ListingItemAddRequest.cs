namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems
{
	public class ListingItemAddRequest
	{
		public Guid ListingId { get; set; }
		public Guid CatalogId { get; set; }
		public string? Details { get; set; }
	}
}
