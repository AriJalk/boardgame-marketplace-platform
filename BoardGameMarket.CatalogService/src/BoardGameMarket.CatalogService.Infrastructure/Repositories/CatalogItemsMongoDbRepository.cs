using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.Enums;
using BoardGameMarket.CatalogService.Core.Domain.RepositoryContracts;
using BoardGameMarket.CatalogService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BoardGameMarket.CatalogService.Infrastructure.Repositories
{
    internal class CatalogItemsMongoDbRepository : ICatalogItemsRepository
	{
		private readonly IMongoCollection<CatalogItem> _items;

		public CatalogItemsMongoDbRepository(IMongoDatabase database, IOptions<MongoDbConfiguration> mongoDbConfiguration)
		{
			_items = database.GetCollection<CatalogItem>(mongoDbConfiguration.Value.CollectionName);
		}
		public async Task<CatalogItem?> GetItemByDatabaseIdAsync(Guid id)
		{
			FilterDefinition<CatalogItem> filter = Builders<CatalogItem>.Filter.Eq(i => i.Id, id);
			return await _items.Find(filter).FirstOrDefaultAsync();

		}

		public async Task<CatalogItem?> GetBggItemByBggIdAsync(int id)
		{
			FilterDefinition<CatalogItem> filter = Builders<CatalogItem>.Filter.Eq(i => i.BggId, id);
			return await _items.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<CatalogItem?> AddCatalogItemToDatabaseAsync(CatalogItem bggItem)
		{
			await _items.InsertOneAsync(bggItem);
			return bggItem;
		}

		public async Task<CatalogItem?> GetCustomItemByNameAsync(string name)
		{
			FilterDefinition<CatalogItem> filter =  Builders<CatalogItem>.Filter.Eq(i => i.Type, CatalogItemType.Custom) & Builders<CatalogItem>.Filter.Eq(i => i.Name, name);
			return await _items.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync()
		{
			FilterDefinition<CatalogItem> filter = Builders<CatalogItem>.Filter.Empty;
			return await _items.Find(filter).ToListAsync();
		}

        public async Task<IEnumerable<CatalogItem>> GetCustomItemsByNamesAsync(IEnumerable<string> names)
        {
			FilterDefinition<CatalogItem> filter = Builders<CatalogItem>.Filter.In(i => i.Name, names);

			return await _items.Find(filter).ToListAsync();
        }	
    }
}
