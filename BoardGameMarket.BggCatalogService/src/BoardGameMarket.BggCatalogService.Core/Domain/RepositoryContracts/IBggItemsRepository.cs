using BoardGameMarket.BggCatalogService.Core.Domain.Entities;

namespace BoardGameMarket.BggCatalogService.Core.Domain.RepositoryContracts
{
	public interface IBggItemsRepository
	{
		Task<BggItem?> GetItemByDatabaseIdAsync(Guid id);
		Task<BggItem?> GetItemByBggIdAsync(int id);
		Task<BggItem?> AddBggItemToDatabaseAsync(BggItem bggItem);
	}
}
