namespace TestingService.Dtos.Item
{
	public class ListingItemDto
	{
		public Guid Id { get; set; }
		public CatalogItemDto? CatalogItem { get; set; }
		public string? Details { get; set; }
	}
}
