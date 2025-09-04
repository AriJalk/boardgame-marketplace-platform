using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGames;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGames
{
	public interface IListingGameAdderService
	{
		Task<ListingGame?> AddListingGame(ListingGameAddRequest? listingGameAddRequest);
	}
}
