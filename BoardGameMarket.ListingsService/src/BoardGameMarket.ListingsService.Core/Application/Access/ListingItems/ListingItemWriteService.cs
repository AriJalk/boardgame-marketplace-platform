using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingItems
{
	internal class ListingItemWriteService : IListingItemWriteService
	{
		private readonly IListingItemRepository _listingItemRepository;

		public ListingItemWriteService(IListingItemRepository listingItemRepository)
		{
			_listingItemRepository = listingItemRepository;
		}

		public async Task AddListingItemAsync(ListingItem listingItemToAdd)
		{
			int rows = await _listingItemRepository.AddListingItemAsync(listingItemToAdd);
			if (rows == 0)
			{
				throw new InvalidOperationException();
			}
		}

		public async Task AddListingItemsAsync(IEnumerable<ListingItem> listingItemsToAdd)
		{
			List<ListingItem> listingItems = listingItemsToAdd.ToList();
			int rows = await _listingItemRepository.AddListingItemsAsync(listingItems);
			if (rows != listingItems.Count)
			{
				throw new InvalidOperationException();
			}
		}

		public async Task DeleteListingItemAsync(Guid listingItemId)
		{
			await _listingItemRepository.DeleteListingItemAsync(listingItemId);
		}

		public async Task DeleteListingItemsByListingIdAsync(Guid listingId)
		{
			await _listingItemRepository.DeleteListingItemsByListingIdAsync(listingId);
		}

        public async Task UpdateListingItemAsync(ListingItem listingItem)
        {
            int rows = await _listingItemRepository.UpdateListingItemAsync(listingItem);
			if (rows == 0)
			{
				throw new InvalidOperationException();
			}
        }
    }
}
