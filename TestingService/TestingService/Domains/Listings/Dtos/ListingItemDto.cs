using TestingService.Domains.CatalogItems.Dtos;

namespace TestingService.Domains.Listings.Dtos
{
	public class ListingItemDto
	{
		public Guid Id { get; set; }
		public CatalogItemDto? CatalogItem { get; set; }
		public string? Details { get; set; }
	}
}
