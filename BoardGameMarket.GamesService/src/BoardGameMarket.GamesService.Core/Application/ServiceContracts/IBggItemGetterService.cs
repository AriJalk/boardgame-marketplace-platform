using BoardGameMarket.GamesService.Core.Domain.Entities;

namespace BoardGameMarket.GamesService.Core.Application.ServiceContracts
{
	public interface IBggItemGetterService
	{
		Task<BggItem?> GetItemByDatabaseId(Guid id);
		Task<IEnumerable<BggItem>> GetItemsByBggIds(IEnumerable<int> bggIds);
	}
}
