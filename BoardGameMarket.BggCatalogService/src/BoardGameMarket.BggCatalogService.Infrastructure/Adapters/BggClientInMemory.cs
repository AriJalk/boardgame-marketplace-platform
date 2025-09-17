using BoardGameMarket.BggCatalogService.Core.Application.AdapterContracts;
using BoardGameMarket.BggCatalogService.Core.Domain.Entities;
using BoardGameMarket.BggCatalogService.Core.Domain.Enums;
using BoardGameMarket.BggCatalogService.Core.Domain.ValueObjects;

namespace BoardGameMarket.BggCatalogService.Infrastructure.Adapters
{
	internal class BggClientInMemory : IBggClient
	{
		private List<RawBggMetadata> _metadataList;

		public BggClientInMemory()
		{
			_metadataList = new List<RawBggMetadata>();
			_metadataList.Add(new RawBggMetadata() { BggId = 1, Categories = new List<Category> { new Category() { Id = 1, Name = "BG" } }, Name = "Game1" });
			_metadataList.Add(new RawBggMetadata() { BggId = 2, Categories = new List<Category> { new Category() { Id = 1, Name = "BG" } }, Name = "Game2" }); _metadataList.Add(new RawBggMetadata() { BggId = 3, Categories = new List<Category> { new Category() { Id = 1, Name = "BG" } }, Name = "Game3" });
		}


		public async Task<IEnumerable<RawBggMetadata>?> GetMultiBggMetadataAsync(IEnumerable<int> bggIds)
		{
			return _metadataList.Where(m => bggIds.Contains(m.BggId));
		}
	}
}
