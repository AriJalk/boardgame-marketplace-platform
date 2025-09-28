using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs
{
	public class ListingItemDtoComposer
	{
		private readonly IListingItemGetterService _listingItemGetterService;

		public ListingItemDtoComposer(IListingItemGetterService listingItemGetterService)
		{
			_listingItemGetterService = listingItemGetterService;
		}

		public async Task<ListingItemResponseDto[]> ComposeListingItemsAsync(Guid listingId)
		{
			IEnumerable
				<ListingItem> listingItems = await _listingItemGetterService.GetListingItemsByListingIdAsync(listingId);
			return listingItems.Select(i => i.ToDto()).ToArray();
		}
	}
}
