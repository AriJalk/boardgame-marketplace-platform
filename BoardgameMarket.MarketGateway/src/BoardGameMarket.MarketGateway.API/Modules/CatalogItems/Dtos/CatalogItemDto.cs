using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Enums;

namespace BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Dtos
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
