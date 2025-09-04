using BoardGameMarket.GamesService.Core.Application.ServiceContracts;
using BoardGameMarket.GamesService.Core.Domain.Entities;
using BoardGameMarket.GamesService.Core.Domain.ValueObjects;

namespace BoardGameMarket.GamesService.Core.Application.Services
{
	public class BggItemFactory : IBggItemFactory
	{
		public BggItem CreateFromMetadata(BggMetadata metadata)
		{
			return new BggItem()
			{
				Id = Guid.NewGuid(),
				BggMetadata = metadata,
				ModifiedTimestamp = DateTime.UtcNow,
			};
		}
	}
}
