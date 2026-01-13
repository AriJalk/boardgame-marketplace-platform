using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups
{
	internal class ListingGroupAdderService : IListingGroupAdderService
	{
		private readonly IListingGroupRepository _listingGroupRepository;

		public ListingGroupAdderService(IListingGroupRepository listingGroupRepository)
		{
			_listingGroupRepository = listingGroupRepository;
		}

		public async Task<ListingGroup?> AddListingGroup(ListingGroup listingGroupToAdd)
		{
			listingGroupToAdd.Id = Guid.NewGuid();
			listingGroupToAdd.CreatedAt = DateTime.UtcNow;
			listingGroupToAdd.ModifiedAt = DateTime.UtcNow;
			return await _listingGroupRepository.AddListingGroupAsync(listingGroupToAdd);
		}
	}
}
