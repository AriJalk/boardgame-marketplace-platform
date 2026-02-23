using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.Enums;
using BoardGameMarket.CatalogService.Core.Domain.RepositoryContracts;
using MongoDB.Driver;

namespace BoardGameMarket.CatalogService.Infrastructure.Repositories
{
    public class CatalogItemsInMemoryRepository : ICatalogItemsRepository
    {
        private readonly List<CatalogItem> _items;

        public CatalogItemsInMemoryRepository()
        {
            _items = new List<CatalogItem>();
        }
        public async Task<CatalogItem?> GetItemByDatabaseIdAsync(Guid id)
        {
            return _items.Where(i => i.Id == id).FirstOrDefault();
        }

        public async Task<CatalogItem?> GetBggItemByBggIdAsync(int id)
        {
            return _items.Where(i => i.BggId == id).FirstOrDefault();
        }

        public async Task<CatalogItem?> AddCatalogItemToDatabaseAsync(CatalogItem bggItem)
        {
            _items.Add(bggItem);
            return bggItem;
        }

        public async Task<CatalogItem?> GetCustomItemByNameAsync(string name)
        {
            return _items.Where(i => i.Type == CatalogItemType.Custom).Where(i => i.Name == name).FirstOrDefault();
        }

        public async Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync()
        {
            return _items;
        }

        public async Task<IEnumerable<CatalogItem>> GetCustomItemsByNamesAsync(IEnumerable<string> names)
        {
            return _items.Where(i => i.Type == CatalogItemType.Custom && names.Contains(i.Name));
        }
    }
}
