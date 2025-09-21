using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	internal static class ListingItemMappers
	{
		public static ListingItemResponseDto ToDto(this ListingItem listingItem)
		{
			return new ListingItemResponseDto()
			{
				ListingId = listingItem.ListingId,
				CatalogId = listingItem.CatalogId,
			};
		}

		public static ListingItem ToListingItem(this ListingItemAddRequest listingItemAddRequest)
		{
			return new ListingItem()
			{
				CatalogId = listingItemAddRequest.CatalogId,
				ListingId = listingItemAddRequest.ListingId,
			};
		}
	}
}
