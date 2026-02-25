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

		public async Task AddListingGroup(ListingGroup listingGroupToAdd)
		{
			int rows = await _listingGroupRepository.AddListingGroupAsync(listingGroupToAdd);
			if (rows == 0)
			{
				throw new InvalidOperationException();
			}
		}

		public async Task DeleteListingGroup(Guid listingGroupId)
		{
			await _listingGroupRepository.DeleteListingGroupAsync(listingGroupId);
		}


		public async Task UpdateListingGroupAsync(ListingGroup listingGroupToUpdate)
		{
			int rows = await _listingGroupRepository.UpdateListingGroupAsync(listingGroupToUpdate);
			if (rows == 0)
			{
				throw new InvalidOperationException();
			}

		}
	}
}
