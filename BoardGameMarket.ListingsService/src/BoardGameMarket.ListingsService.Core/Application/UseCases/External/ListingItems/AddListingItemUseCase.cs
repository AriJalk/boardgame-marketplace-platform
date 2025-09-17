using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingItems
{
	internal class AddListingItemUseCase
	{
		private readonly ILogger<AddListingItemUseCase> _logger;
		private readonly IListingItemAdderService _listingItemAdderService;
		private readonly ListingFetcher _listingFetcher;
		private readonly ICurrentUserContext _currentUserContext;

		public AddListingItemUseCase(ILogger<AddListingItemUseCase> logger, IListingItemAdderService listingItemAdderService, ListingFetcher listingFetcher, ICurrentUserContext currentUserContext)
		{
			_logger = logger;
			_listingItemAdderService = listingItemAdderService;
			_listingFetcher = listingFetcher;
			_currentUserContext = currentUserContext;
		}

		public async Task<ListingItemResponseDto> ExecuteAsync(ListingItemAddRequest listingItemAddRequest)
		{
			Guid sellerId = await _listingFetcher.GetSellerUserIdByListingIdAsync(listingItemAddRequest.ListingId);
			IdentityHelper.AuthorizeCurrentUserOrAdmin(sellerId, _currentUserContext);
			ListingItem? listingItem = await _listingItemAdderService.AddListingItemAsync(listingItemAddRequest.ToListingItem());
			if (listingItem == null)
			{
				throw new InvalidOperationException("Can't create listing item");
			}
			return listingItem.ToDto();
		}
	}
}
