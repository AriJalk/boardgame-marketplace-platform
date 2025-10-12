using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingItems
{
	public class DeleteListingItemUseCase
	{
		private readonly IListingItemGetterService _listingItemGetterService;
		private readonly IListingItemDeleterService _listingItemDeleterService;
		private readonly ListingFetcher _listingFetcher;
		private readonly ICurrentUserContext _currentUserContext;

		public DeleteListingItemUseCase(IListingItemGetterService listingItemGetterService, IListingItemDeleterService listingItemDeleterService, ListingFetcher listingFetcher, ICurrentUserContext currentUserContext)
		{
			_listingItemGetterService = listingItemGetterService;
			_listingItemDeleterService = listingItemDeleterService;
			_listingFetcher = listingFetcher;
			_currentUserContext = currentUserContext;
		}

		public async Task<bool> Execute(Guid listingItemId)
		{
			ListingItem? listingItem = await _listingItemGetterService.GetListingItemById(listingItemId);
			if (listingItem == null)
			{
				throw new KeyNotFoundException();
			}
			Listing listing = await _listingFetcher.GetListingByIdAsync(listingItem.ListingId);
			ListingGroup listingGroup = await _listingFetcher.GetListingGroupByIdAsync(listing.ListingGroupId);
			_currentUserContext.ValidateSameUserOrAtLeastRole(listingGroup.SellerUserId, RoleType.Moderator);
			bool deleted = await _listingItemDeleterService.DeleteListingItemAsync(listingItemId);
			if (!deleted)
				throw new InvalidOperationException("Listing item not found or already deleted.");
			return deleted;

		}
	}
}
