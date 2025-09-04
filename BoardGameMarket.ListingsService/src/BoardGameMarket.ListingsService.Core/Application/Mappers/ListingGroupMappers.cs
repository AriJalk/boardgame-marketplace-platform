using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	internal static class ListingGroupMappers
	{
		public static ListingGroupResponseDto ToDto(this ListingGroup listingGroup)
		{
			return new ListingGroupResponseDto()
			{
				Id = listingGroup.Id,
				SellerUserId = listingGroup.SellerUserId,
				PickupOptions = listingGroup.PickupOptions,
				Details = listingGroup.Details,
				Location = listingGroup.Location,
			};
		}
	}
}
