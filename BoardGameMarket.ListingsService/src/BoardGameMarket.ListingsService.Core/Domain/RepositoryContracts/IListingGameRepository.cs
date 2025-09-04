using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingGameRepository
	{
		Task<ListingGame?> GetListingGameByIdAsync(Guid listingGameId);
		Task<IEnumerable<ListingGame>> GetListingsGamesByListingIdAsync(Guid listingId);
		Task<ListingGame?> AddListingGameAsync(ListingGame listingGame);
		Task<bool?> DeleteListingGame(Guid listingGameId);
	}
}
