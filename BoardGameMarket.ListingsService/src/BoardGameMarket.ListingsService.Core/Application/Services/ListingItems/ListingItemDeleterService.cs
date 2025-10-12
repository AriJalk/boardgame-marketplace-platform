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
	internal class ListingItemDeleterService : IListingItemDeleterService
	{
		private readonly IListingItemRepository _listingItemRepository;

		public ListingItemDeleterService(IListingItemRepository listingItemRepository)
		{
			_listingItemRepository = listingItemRepository;
		}

		public async Task<bool> DeleteListingItemAsync(Guid listingItemId)
		{
			return await _listingItemRepository.DeleteListingItemAsync(listingItemId);
		}

		public async Task DeleteListingItemsByListingIdAsync(Guid listingId)
		{
			await _listingItemRepository.DeleteListingItemsByListingIdAsync(listingId);
		}
	}
}
