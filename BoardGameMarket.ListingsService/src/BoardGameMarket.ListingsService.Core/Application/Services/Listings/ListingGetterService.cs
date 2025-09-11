using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	internal class ListingGetterService : IListingGetterService
	{
		private readonly IListingRepository _listingRepository;
		private readonly ILogger<ListingGetterService> _logger;

		public ListingGetterService(IListingRepository listingRepository, ILogger<ListingGetterService> logger)
		{
			_listingRepository = listingRepository;
			_logger = logger;
		}

		public async Task<Listing?> GetListingByIdAsync(Guid listingId)
		{
			_logger.LogInformation($"{nameof(ListingGetterService)}:{nameof(GetListingByIdAsync)}- get listing {listingId}");

			return await _listingRepository.GetListingByIdAsync(listingId);
		}

		public async Task<IEnumerable<Listing>> GetListingsByListingGroupIdAsync(Guid groupId)
		{
			_logger.LogInformation($"{nameof(ListingGetterService)}:{nameof(GetListingsByListingGroupIdAsync)}- get listing by group {groupId}");

			return await _listingRepository.GetListingsByGroupIdAsync(groupId);
		}
	}
}
