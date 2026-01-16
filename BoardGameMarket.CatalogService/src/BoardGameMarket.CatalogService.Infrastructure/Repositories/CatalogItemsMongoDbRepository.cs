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

		public CatalogItemsMongoDbRepository(IMongoClient mongoClient, IOptions<MongoDbConfiguration> mongoDbConfiguration)
		{
			_items = mongoClient.GetDatabase(mongoDbConfiguration.Value.DatabaseName).GetCollection<CatalogItem>(mongoDbConfiguration.Value.CollectionName);
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

		public Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync()
		{
			throw new NotImplementedException();
		}
	}
}
