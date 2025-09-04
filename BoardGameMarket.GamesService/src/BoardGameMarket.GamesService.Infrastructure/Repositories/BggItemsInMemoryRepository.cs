using BoardGameMarket.GamesService.Core.Domain.Entities;
using BoardGameMarket.GamesService.Core.Domain.RepositoryContracts;
using MongoDB.Driver;

namespace BoardGameMarket.GamesService.Infrastructure.Repositories
{
	public class BggItemsInMemoryRepository : IBggItemsRepository
	{
		private readonly List<BggItem> _items;

		public BggItemsInMemoryRepository()
		{
			_items = new List<BggItem>();
		}
		public async Task<BggItem?> GetItemByDatabaseIdAsync(Guid id)
		{
			return _items.Where(i => i.Id == id).FirstOrDefault();
		}

		public async Task<BggItem?> GetItemByBggIdAsync(int id)
		{
			return _items.Where(i => i.BggMetadata.BggId == id).FirstOrDefault();
		}

		public async Task<BggItem?> AddBggItemToDatabaseAsync(BggItem bggItem)
		{
			_items.Add(bggItem);
			return bggItem;
		}


	}
}
