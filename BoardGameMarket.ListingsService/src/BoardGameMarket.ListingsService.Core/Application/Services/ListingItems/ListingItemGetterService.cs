using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingItems
{
	internal class ListingItemGetterService : IListingItemGetterService
	{
		private readonly IListingItemRepository _listingItemRepository;
		private readonly ILogger<ListingItemGetterService> _logger;

		public ListingItemGetterService(IListingItemRepository listingItemRepository, ILogger<ListingItemGetterService> logger)
		{
			_listingItemRepository = listingItemRepository;
			_logger = logger;
		}

		public async Task<ListingItem?> GetListingItemById(Guid listingItemId)
		{
			_logger.LogInformation($"{nameof(ListingItemGetterService)}:{nameof(GetListingItemById)}- get listing item {listingItemId}");
			return await _listingItemRepository.GetListingItemByIdAsync(listingItemId);
		}

		public async Task<IEnumerable<ListingItem>> GetListingItemsByListingIdAsync(Guid listingId)
		{
			_logger.LogInformation($"{nameof(ListingItemGetterService)}:{nameof(GetListingItemById)}- get listing items by listing {listingId}");
			return await _listingItemRepository.GetListingsItemsByListingIdAsync(listingId);
		}
	}
}
