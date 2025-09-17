using BoardGameMarket.BggCatalogService.Core.Domain.Entities;
using BoardGameMarket.BggCatalogService.Core.Domain.RepositoryContracts;
using BoardGameMarket.BggCatalogService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BoardGameMarket.BggCatalogService.Infrastructure.Repositories
{
	internal class BggItemsMongoDbRepository : IBggItemsRepository
	{
		private readonly IMongoCollection<BggItem> _items;

		public BggItemsMongoDbRepository(IMongoClient mongoClient, IOptions<MongoDbConfiguration> mongoDbConfiguration)
		{
			_items = mongoClient.GetDatabase(mongoDbConfiguration.Value.DatabaseName).GetCollection<BggItem>(mongoDbConfiguration.Value.CollectionName);
		}
		public async Task<BggItem?> GetItemByDatabaseIdAsync(Guid id)
		{
			FilterDefinition<BggItem> filter = Builders<BggItem>.Filter.Eq(i => i.Id, id);
			return await _items.Find(filter).FirstOrDefaultAsync();

		}

		public async Task<BggItem?> GetItemByBggIdAsync(int id)
		{
			FilterDefinition<BggItem> filter = Builders<BggItem>.Filter.Eq(i => i.BggMetadata.BggId, id);
			return await _items.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<BggItem?> AddBggItemToDatabaseAsync(BggItem bggItem)
		{
			await _items.InsertOneAsync(bggItem);
			return bggItem;
		}


	}
}
