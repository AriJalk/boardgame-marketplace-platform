using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
	internal class ListingGameRepository : IListingGameRepository
	{
		public Task<ListingGame?> AddListingGameAsync(ListingGame listingGame)
		{
			throw new NotImplementedException();
		}

		public Task<bool?> DeleteListingGame(Guid listingGameId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingGame?> GetListingGameByIdAsync(Guid listingGameId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingGame>> GetListingsGamesByListingIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}
	}
}
