using BoardGameMarket.GamesService.Core.Application.AdapterContracts;
using BoardGameMarket.GamesService.Core.Domain.Entities;
using BoardGameMarket.GamesService.Core.Domain.Enums;
using BoardGameMarket.GamesService.Core.Domain.ValueObjects;

namespace BoardGameMarket.GamesService.Infrastructure.Adapters
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
