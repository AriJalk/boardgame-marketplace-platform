using BoardGameMarket.BggCatalogService.Core.Domain.Entities;

namespace BoardGameMarket.BggCatalogService.Core.Application.ServiceContracts
{
	public interface IBggItemGetterService
	{
		Task<BggItem?> GetItemByDatabaseId(Guid id);
		Task<IEnumerable<BggItem>> GetItemsByBggIds(IEnumerable<int> bggIds);
		Task<IEnumerable<BggItem>> GetItemsByDatabaseIds(IEnumerable<Guid> databaseIds);
	}
}
