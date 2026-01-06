namespace TestingService.Dtos
{
	public class ListingItemDto
	{
		public Guid Id { get; set; }
		public CatalogItemDto? CatalogItem { get; set; }
		public string? details { get; set; }
	}
}
