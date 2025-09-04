using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGames;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGames;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGames
{
	public class ListingGameAdderService : IListingGameAdderService
	{
		private readonly IListingGameRepository _listingGameRepository;
		private readonly ILogger<ListingGameAdderService> _logger;

		public ListingGameAdderService(ILogger<ListingGameAdderService> logger, IListingGameRepository listingGameRepository)
		{
			_listingGameRepository = listingGameRepository;
			_logger = logger;
		}

		public async Task<ListingGame?> AddListingGame(ListingGameAddRequest? listingGameAddRequest)
		{
			if (listingGameAddRequest == null)
			{
				return null;
			}

			ListingGame listingGame = new ListingGame()
			{
				Id = Guid.NewGuid(),
				ListingId = listingGameAddRequest.ListingId,
				GameId = listingGameAddRequest.GameId,
			};

			ListingGame? response = await _listingGameRepository.AddListingGameAsync(listingGame);

			return await _listingGameRepository.AddListingGameAsync(listingGame);
		}
	}
}
