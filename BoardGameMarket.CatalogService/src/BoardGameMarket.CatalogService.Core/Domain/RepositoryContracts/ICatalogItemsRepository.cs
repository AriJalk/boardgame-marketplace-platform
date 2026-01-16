using BoardGameMarket.CatalogService.Core.Domain.Entities;

namespace BoardGameMarket.CatalogService.Core.Domain.RepositoryContracts
{
	public interface ICatalogItemsRepository
	{
		Task<CatalogItem?> GetItemByDatabaseIdAsync(Guid id);
		Task<CatalogItem?> GetBggItemByBggIdAsync(int id);
		Task<CatalogItem?> GetCustomItemByNameAsync(string name);
		Task<CatalogItem?> AddCatalogItemToDatabaseAsync(CatalogItem bggItem);
		Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync();
	}
}
