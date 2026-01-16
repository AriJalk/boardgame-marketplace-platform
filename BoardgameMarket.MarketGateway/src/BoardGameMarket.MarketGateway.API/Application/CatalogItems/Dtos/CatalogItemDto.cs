using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Enums;

namespace BoardGameMarket.MarketGateway.API.Application.CatalogItems.Dtos
{
	public class CatalogItemDto
	{
		public Guid Id { get; set; }
		public string? Name { get; set; }
		public CatalogItemType Type { get; set; }
		public string? BggUrl { get; set; }
		public string? ImageUrl { get; set; }
	}
}
