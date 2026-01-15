using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Enums;

namespace BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients
{
	public record CatalogItemInfo(Guid Id, string Name, CatalogItemType Type, int? BggId, string? ImageUrl)
	{
	}
}
