using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups
{
	internal class ListingGroupUpdaterService : IListingGroupUpdaterService
	{
		private readonly IListingGroupRepository _listingGroupRepository;
		private readonly ILogger<ListingGroupAdderService> _logger;

		public ListingGroupUpdaterService(IListingGroupRepository listingGroupRepository, ILogger<ListingGroupAdderService> logger)
		{
			_listingGroupRepository = listingGroupRepository;
			_logger = logger;
		}

		public async Task<ListingGroup?> UpdateListingGroupAsync(ListingGroup listingGroupToUpdate)
		{
			_logger.LogInformation($"{nameof(ListingGroupUpdaterService)}:{nameof(UpdateListingGroupAsync)}- updating listing group {listingGroupToUpdate.Id}");
			return await _listingGroupRepository.UpdateListingGroupAsync(listingGroupToUpdate);

		}
	}
}
