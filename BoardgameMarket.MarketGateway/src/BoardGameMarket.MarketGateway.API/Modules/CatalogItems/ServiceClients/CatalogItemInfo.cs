using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Enums;

namespace BoardGameMarket.MarketGateway.API.Modules.CatalogItems.ServiceClients
{
	public record CatalogItemInfo(Guid Id, string Name, CatalogItemType Type, int? BggId, string? ImageUrl)
	{
	}
}
