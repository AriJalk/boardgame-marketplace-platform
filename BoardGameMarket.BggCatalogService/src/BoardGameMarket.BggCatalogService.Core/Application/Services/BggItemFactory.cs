using BoardGameMarket.BggCatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.BggCatalogService.Core.Domain.Entities;
using BoardGameMarket.BggCatalogService.Core.Domain.ValueObjects;

namespace BoardGameMarket.BggCatalogService.Core.Application.Services
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
