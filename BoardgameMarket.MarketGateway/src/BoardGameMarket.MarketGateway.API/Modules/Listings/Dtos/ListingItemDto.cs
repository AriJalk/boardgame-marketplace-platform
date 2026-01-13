using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Dtos;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos
{
	public class ListingItemDto
	{
		public Guid Id { get; set; }
		public CatalogItemDto? CatalogItem { get; set; }
		public string? Details { get; set; }
	}
}
