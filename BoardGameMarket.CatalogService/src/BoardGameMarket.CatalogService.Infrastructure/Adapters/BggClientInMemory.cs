using BoardGameMarket.CatalogService.Core.Application.AdapterContracts;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.Enums;
using BoardGameMarket.CatalogService.Core.Domain.ValueObjects;
using System.Text.Json;

namespace BoardGameMarket.CatalogService.Infrastructure.Adapters
{
	internal class BggClientInMemory : IBggClient
	{
		private readonly Dictionary<int, RawBggMetadata> _items = new Dictionary<int, RawBggMetadata>();

		public BggClientInMemory() 
		{
			_items.Add(1, new RawBggMetadata(1, "First entry"));
		}
		public async Task<IEnumerable<RawBggMetadata>> GetMultiBggMetadataAsync(IEnumerable<int> bggIds)
		{
			List<RawBggMetadata> list = new List<RawBggMetadata>();
			foreach (int bggId in bggIds)
			{
				if (_items.TryGetValue(bggId, out RawBggMetadata? data))
				{
					list.Add(data);
				}
			}
			return list;
		}
	}
}
