using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups
{
	internal class ListingGroupWriteService : IListingGroupWriteService
	{
		private readonly IListingGroupRepository _listingGroupRepository;

		public ListingGroupWriteService(IListingGroupRepository listingGroupRepository)
		{
			_listingGroupRepository = listingGroupRepository;
		}

		public async Task<ListingGroup?> AddListingGroup(ListingGroup listingGroupToAdd)
		{
			return await _listingGroupRepository.AddListingGroupAsync(listingGroupToAdd);
		}

		public async Task<bool> DeleteListingGroup(Guid listingGroupId)
		{
			return await _listingGroupRepository.DeleteListingGroupAsync(listingGroupId);
		}


		public async Task<ListingGroup?> UpdateListingGroupAsync(ListingGroup listingGroupToUpdate)
		{
			return await _listingGroupRepository.UpdateListingGroupAsync(listingGroupToUpdate);

		}
	}
}
