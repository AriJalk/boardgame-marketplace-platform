using BoardGameMarket.GamesService.Core.Domain.ValueObjects;

namespace BoardGameMarket.GamesService.Core.Domain.Entities
{
	public class BggItem
	{
		public Guid Id { get; set; }
		public BggMetadata BggMetadata { get; set; } = new BggMetadata();
		public DateTime ModifiedTimestamp { get; set; }
	}
}
