using TestingService.Domains.CatalogItems.Enums;

namespace TestingService.Domains.CatalogItems.ServiceClients
{
	public record CatalogItemInfo(Guid Id, string Name, CatalogItemType Type, int? BggId, string? ImageUrl)
	{
	}
}
