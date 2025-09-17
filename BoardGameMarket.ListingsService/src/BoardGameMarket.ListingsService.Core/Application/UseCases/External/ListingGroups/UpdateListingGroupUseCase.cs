using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingGroups
{
	internal class UpdateListingGroupUseCase
	{
		private readonly ListingFetcher _listingFetcher;
		private readonly IListingGroupUpdaterService _listingGroupUpdaterService;
		private readonly ICurrentUserContext _currentUserContext;

		public UpdateListingGroupUseCase(ListingFetcher listingFetcher, IListingGroupUpdaterService listingGroupUpdaterService, ICurrentUserContext currentUserContext)
		{
			_listingFetcher = listingFetcher;
			_listingGroupUpdaterService = listingGroupUpdaterService;
			_currentUserContext = currentUserContext;
		}

		public async Task<ListingGroup> ExecuteAsync(ListingGroupUpdateRequest listingGroupUpdateRequest)
		{
			ListingGroup listingGroup = await _listingFetcher.GetListingGroupByIdAsync(listingGroupUpdateRequest.Id);
			IdentityHelper.AuthorizeCurrentUserOrAdmin(listingGroup.SellerUserId, _currentUserContext);
			bool isChanged = false;
			if (listingGroup.Details != listingGroupUpdateRequest.Details)
			{
				isChanged = true;
				listingGroup.Details = listingGroupUpdateRequest.Details;
			}
			if (listingGroup.Location != listingGroupUpdateRequest.Location)
			{
				isChanged = true;
				listingGroup.Location = listingGroupUpdateRequest.Location;
			}
			if (listingGroup.PickupOptions != listingGroupUpdateRequest.PickupOptions)
			{
				isChanged = true;
				listingGroup.PickupOptions = listingGroupUpdateRequest.PickupOptions;
			}

			if (!isChanged)
			{
				throw new InvalidOperationException("No changes were made");
			}
			ListingGroup? listingGroupFromUpdate = await _listingGroupUpdaterService.UpdateListingGroupAsync(listingGroup);
			if (listingGroupFromUpdate == null)
			{
				throw new InvalidOperationException("Can't update listing group");
			}
			return listingGroupFromUpdate;
		}
	}
}