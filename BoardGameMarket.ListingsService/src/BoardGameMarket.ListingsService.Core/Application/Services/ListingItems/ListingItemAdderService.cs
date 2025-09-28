using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingItems
{
	internal class ListingItemAdderService : IListingItemAdderService
	{
		private readonly IListingItemRepository _listingItemRepository;

		public ListingItemAdderService(IListingItemRepository listingItemRepository)
		{
			_listingItemRepository = listingItemRepository;
		}

		public async Task<ListingItem?> AddListingItemAsync(ListingItem listingItemToAdd)
		{
			listingItemToAdd.Id = Guid.NewGuid();
			return await _listingItemRepository.AddListingItemAsync(listingItemToAdd);
		}

		public async Task<IEnumerable<ListingItem>> AddListingItemsAsync(IEnumerable<ListingItem> listingItemsToAdd)
		{
			foreach (ListingItem item in listingItemsToAdd)
			{
				item.Id = Guid.NewGuid();
			}
			return await _listingItemRepository.AddListingItemsAsync(listingItemsToAdd);
		}
	}
}
