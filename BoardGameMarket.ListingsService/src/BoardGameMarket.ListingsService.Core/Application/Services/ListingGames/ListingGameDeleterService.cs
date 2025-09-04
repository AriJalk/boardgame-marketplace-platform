using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGames
{
	public class ListingGameDeleterService : IListingGameDeleterService
	{
		public Task<bool> DeleteListingGame(Guid listingGameId)
		{
			throw new NotImplementedException();
		}
	}
}
