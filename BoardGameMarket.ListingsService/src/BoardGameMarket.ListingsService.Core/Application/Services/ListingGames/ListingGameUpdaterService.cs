using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGames;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGames;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGames
{
	public class ListingGameUpdaterService : IListingGameUpdaterService
	{
		public Task<ListingGame?> UpdateListingGame(ListingGameUpdateRequest? listingGameUpdateRequest)
		{
			throw new NotImplementedException();
		}
	}
}
