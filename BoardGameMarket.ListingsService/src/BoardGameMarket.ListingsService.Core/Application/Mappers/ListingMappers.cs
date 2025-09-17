using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	internal static class ListingMappers
	{
		public static ListingResponseDto ToDto(this Listing listing)
		{
			return new ListingResponseDto()
			{
				Id = listing.Id,
				Status = listing.Status,
				CreatedAt = listing.CreatedAt,
				ModifiedAt = listing.ModifiedAt,
				Price = listing.Price,
				Details = listing.Details,
				ListingGroupId = listing.ListingGroupId,
			};
		}

		public static Listing ToListing(this ListingAddRequest listingAddRequest)
		{
			return new Listing()
			{
				ListingGroupId = listingAddRequest.ListingGroupId,
				Price = listingAddRequest.Price,
				Details = listingAddRequest.Details,
				Status = listingAddRequest.Status,
			};
		}

		public static ListingUpdateRequest ToUpdateRequest(this Listing listing)
		{
			return new ListingUpdateRequest()
			{
				Id = listing.Id,
				Status = listing.Status,
				Details = listing.Details,
				Price = listing.Price,
			};
		}
	}
}
