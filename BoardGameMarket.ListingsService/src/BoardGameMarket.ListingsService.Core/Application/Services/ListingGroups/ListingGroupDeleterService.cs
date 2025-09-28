using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups
{
	internal class ListingGroupDeleterService : IListingGroupDeleterService
	{
		private readonly IListingGroupRepository _listingGroupRepository;
		public ListingGroupDeleterService(IListingGroupRepository listingGroupRepository)
		{
			_listingGroupRepository = listingGroupRepository;
		}

		public async Task<bool> DeleteListingGroup(Guid listingGroupId)
		{
			return await _listingGroupRepository.DeleteListingGroup(listingGroupId);
		}
	}
}
