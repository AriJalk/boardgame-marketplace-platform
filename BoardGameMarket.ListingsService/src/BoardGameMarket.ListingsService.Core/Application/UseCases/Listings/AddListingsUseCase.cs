using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.Listings
{
	public class AddListingsUseCase
	{
		private readonly IListingAdderService _listingAdderService;
		private readonly ILogger<IListingAdderService> _logger;

		public AddListingsUseCase(IListingAdderService listingAdderService, ILogger<IListingAdderService> logger)
		{
			_listingAdderService = listingAdderService;
			_logger = logger;
		}

		public async Task<IEnumerable<ListingResponseDto>> ExecuteAsync(IEnumerable<ListingAddRequest> listingAddRequests)
		{
			IEnumerable<Listing> listings = await _listingAdderService.AddListingsAsync(listingAddRequests);
			return listings.Select(l => l.ToDto());
		}
	}
}
