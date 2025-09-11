using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.Listings
{
	internal class UpdateListingUseCase
	{
		private readonly ListingFetcher _listingFetcher;
		private readonly IListingUpdaterService _listingUpdaterService;
		private readonly ICurrentUserContext _currentUserContext;

		public async Task<Listing> ExecuteAsync(ListingUpdateRequest listingUpdateRequest)
		{
			Listing listing = await _listingFetcher.GetListingAsync(listingUpdateRequest.Id);
			// Validate
			Guid sellerId = await _listingFetcher.GetSellerUserIdByGroupIdAsync(listing.ListingGroupId);
			IdentityHelper.AuthorizeCurrentUserOrAdmin(sellerId, _currentUserContext);

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
			return listingFromUpdate;
		}
	}
}
