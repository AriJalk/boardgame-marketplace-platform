using BoardGameMarket.GamesService.Core.Domain.Entities;

namespace BoardGameMarket.GamesService.Core.Domain.RepositoryContracts
{
	public interface IBggItemsRepository
	{
		Task<BggItem?> GetItemByDatabaseIdAsync(Guid id);
		Task<BggItem?> GetItemByBggIdAsync(int id);
		Task<BggItem?> AddBggItemToDatabaseAsync(BggItem bggItem);
	}
}
