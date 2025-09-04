using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGames;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGames
{
	public class ListingGameGetterService : IListingGameGetterService
	{
		public Task<ListingGame> GetListingGameById(Guid listingGameId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingGame>> GetListingGamesByListingIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}
	}
}
