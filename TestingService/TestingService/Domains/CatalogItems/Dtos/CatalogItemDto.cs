using TestingService.Domains.CatalogItems.Enums;

namespace TestingService.Domains.CatalogItems.Dtos
{
	public class CatalogItemDto
	{
		public Guid Id { get; set; }
		public string? Name { get; set; }
		public CatalogItemType Type { get; set; }
		public string? BggLink { get; set; }
		public string? ImageUrl { get; set; }
	}
}
