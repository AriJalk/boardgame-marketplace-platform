using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.Listings
{
	internal class AddListingUseCase
	{
		private readonly ListingFetcher _listingFetcher;
		private readonly IListingAdderService _listingAdderService;
		private readonly ICurrentUserContext _currentUserContext;

		public AddListingUseCase(ListingFetcher listingFetcher, IListingAdderService listingAdderService, ICurrentUserContext currentUserContext)
		{
			_listingFetcher = listingFetcher;
			_listingAdderService = listingAdderService;
			_currentUserContext = currentUserContext;
		}

		public async Task<ListingResponseDto> ExecuteAsync(ListingAddRequest listingAddRequest)
		{
			//Validate
			Guid sellerId = await _listingFetcher.GetSellerUserIdByGroupIdAsync(listingAddRequest.ListingGroupId);
			IdentityHelper.AuthorizeCurrentUserOrAdmin(sellerId, _currentUserContext);

			Listing? listing = await _listingAdderService.AddListingAsync(listingAddRequest.ToListing());
			if (listing == null)
			{
				throw new InvalidOperationException("Can't create new listing");
			}
			return listing.ToDto();
		}
	}
}
