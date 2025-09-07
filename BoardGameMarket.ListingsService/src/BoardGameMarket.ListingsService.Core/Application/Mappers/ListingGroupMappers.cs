using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups;
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

		public static ListingGroup ToListingGroup(this ListingGroupAddRequest listingGroupAddRequest)
		{
			return new ListingGroup()
			{
				SellerUserId = listingGroupAddRequest.SellerUserId,
				Details = listingGroupAddRequest.Details,
				Location = listingGroupAddRequest.Location,
				PickupOptions = listingGroupAddRequest.PickupOptions,
			};
		}
	}
}
