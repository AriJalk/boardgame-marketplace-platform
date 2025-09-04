using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGames
{
	public interface IListingGameGetterService
	{
		Task<ListingGame> GetListingGameById(Guid listingGameId);
		Task<IEnumerable<ListingGame>> GetListingGamesByListingIdAsync(Guid listingId);
	}
}
