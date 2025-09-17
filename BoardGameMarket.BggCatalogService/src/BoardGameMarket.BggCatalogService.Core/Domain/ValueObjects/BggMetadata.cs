using BoardGameMarket.BggCatalogService.Core.Domain.Enums;

namespace BoardGameMarket.BggCatalogService.Core.Domain.ValueObjects
{
	public class BggMetadata
	{
		public int BggId { get; set; }
		public string? Name { get; set; }
		public List<int> Categories { get; set; } = new List<int>();
	}
}
