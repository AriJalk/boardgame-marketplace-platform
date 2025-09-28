using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.Listings
{
	public class UpdateListingUseCase
	{
		private readonly ListingFetcher _listingFetcher;
		private readonly IListingUpdaterService _listingUpdaterService;

		public UpdateListingUseCase(ListingFetcher listingFetcher, IListingUpdaterService listingUpdaterService)
		{
			_listingFetcher = listingFetcher;
			_listingUpdaterService = listingUpdaterService;
		}

		public async Task<ListingResponseDto> ExecuteAsync(ListingUpdateRequest listingUpdateRequest, Guid listingId)
		{
			Listing listing = await _listingFetcher.GetListingByIdAsync(listingId);
			// Validate
			Guid sellerId = await _listingFetcher.GetSellerUserIdByGroupIdAsync(listing.ListingGroupId);

			bool isChanged = false;
			if (listing.Details != listingUpdateRequest.Details)
			{
				isChanged = true;
				listing.Details = listingUpdateRequest.Details;
			}
			if (listing.Price != listingUpdateRequest.Price)
			{
				isChanged = true;
				listing.Price = listingUpdateRequest.Price;
			}
			if (!isChanged)
			{
				throw new InvalidOperationException("No changes were made");
			}
			Listing? listingFromUpdate = await _listingUpdaterService.UpdateListingAsync(listing);
			if (listingFromUpdate == null)
			{
				throw new InvalidOperationException("Can't update listing");
			}
			return listingFromUpdate.ToDto();
		}
	}
}
