using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingItems
{
	internal class ListingItemDeleterService : IListingItemDeleterService
	{
		private readonly IListingItemRepository _listingItemRepository;
		private readonly ILogger<ListingItemDeleterService> _logger;

		public ListingItemDeleterService(IListingItemRepository listingItemRepository, ILogger<ListingItemDeleterService> logger)
		{
			_listingItemRepository = listingItemRepository;
			_logger = logger;
		}

		public async Task<bool> DeleteListingItem(Guid listingItemId)
		{
			_logger.LogInformation($"{nameof(ListingItemDeleterService)}:{nameof(DeleteListingItem)}- Deleting item {listingItemId}");

			return await _listingItemRepository.DeleteListingItem(listingItemId);
		}
	}
}
