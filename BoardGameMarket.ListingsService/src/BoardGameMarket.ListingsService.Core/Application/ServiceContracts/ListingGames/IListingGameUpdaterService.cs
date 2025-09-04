using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGames;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGames
{
	public interface IListingGameUpdaterService
	{
		Task<ListingGame?> UpdateListingGame(ListingGameUpdateRequest? listingGameUpdateRequest);
	}
}
