using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups
{
	internal class ListingGroupUpdaterService : IListingGroupUpdaterService
	{
		private readonly IListingGroupRepository _listingGroupRepository;

		public ListingGroupUpdaterService(IListingGroupRepository listingGroupRepository)
		{
			_listingGroupRepository = listingGroupRepository;
		}

		public async Task<ListingGroup?> UpdateListingGroupAsync(ListingGroup listingGroupToUpdate)
		{
			return await _listingGroupRepository.UpdateListingGroupAsync(listingGroupToUpdate);

		}
	}
}
