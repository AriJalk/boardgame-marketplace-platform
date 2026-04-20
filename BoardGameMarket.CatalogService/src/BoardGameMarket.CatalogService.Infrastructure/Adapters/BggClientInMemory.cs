using BoardGameMarket.CatalogService.Core.Application.AdapterContracts;
using BoardGameMarket.CatalogService.Core.Domain.ValueObjects;

namespace BoardGameMarket.CatalogService.Infrastructure.Adapters
{
    internal class BggClientInMemory : IBggClient
    {
        private readonly Dictionary<int, RawBggMetadata> _items = new Dictionary<int, RawBggMetadata>();

        public BggClientInMemory()
        {

        }
        public async Task<IEnumerable<RawBggMetadata>> GetMultiBggMetadataAsync(IEnumerable<int> bggIds)
        {
            List<RawBggMetadata> list = new List<RawBggMetadata>();
            foreach (int bggId in bggIds)
            {
                if (!_items.TryGetValue(bggId, out RawBggMetadata? data))
                {
                    data = new RawBggMetadata(bggId, $"Mock Game {bggId}");
                }
                list.Add(data);
            }
            return list;
        }
    }
}
