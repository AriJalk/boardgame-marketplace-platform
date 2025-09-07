using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups
{
	public class ListingGroupUpdaterService : IListingGroupUpdaterService
	{
		private readonly IListingGroupRepository _listingGroupRepository;
		private readonly ILogger<ListingGroupAdderService> _logger;

		public ListingGroupUpdaterService(IListingGroupRepository listingGroupRepository, ILogger<ListingGroupAdderService> logger)
		{
			_listingGroupRepository = listingGroupRepository;
			_logger = logger;
		}

		public async Task<ListingGroup?> UpdateListingGroup(ListingGroupUpdateRequest? listingGroupUpdateRequest)
		{
			ArgumentNullException.ThrowIfNull(listingGroupUpdateRequest, nameof(listingGroupUpdateRequest));

			ListingGroup? listingGroupToUpdate = await _listingGroupRepository.GetListingGroupByIdAsync(listingGroupUpdateRequest.Id);

			if (listingGroupToUpdate == null)
			{
				return null;
			}

			if (listingGroupToUpdate.SellerUserId != listingGroupUpdateRequest.SellerUserId)
			{
				throw new ArgumentException("Mismatch update info");
			}

			bool isChanged = false;
			if (listingGroupToUpdate.Details != listingGroupUpdateRequest.Details)
			{
				isChanged = true;
				listingGroupToUpdate.Details = listingGroupUpdateRequest.Details;
			}

			if (listingGroupToUpdate.Location != listingGroupUpdateRequest.Location)
			{
				isChanged = true;
				listingGroupToUpdate.Location = listingGroupUpdateRequest.Location;
			}

			if (listingGroupToUpdate.PickupOptions != listingGroupUpdateRequest.PickupOptions)
			{
				isChanged = true;
				listingGroupToUpdate.PickupOptions = listingGroupUpdateRequest.PickupOptions;
			}

			if (isChanged)
			{
				_logger.LogInformation($"{nameof(ListingGroupUpdaterService)}:{nameof(UpdateListingGroup)}- updating listing group {listingGroupToUpdate.Id}");
				return await _listingGroupRepository.UpdateListingGroupAsync(listingGroupToUpdate);
			}

			return null;
			
		}
	}
}
