using BoardGameMarket.BggCatalogService.Core.Domain.ValueObjects;

namespace BoardGameMarket.BggCatalogService.Core.Domain.Entities
{
	public class BggItem
	{
		public Guid Id { get; set; }
		public BggMetadata BggMetadata { get; set; } = new BggMetadata();
		public DateTime ModifiedTimestamp { get; set; }
	}
}
