using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Common;
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
	internal class ListingGroupGetterService : IListingGroupGetterService
	{
		private readonly IListingGroupRepository _listingGroupRepository;

		public ListingGroupGetterService(IListingGroupRepository listingGroupRepository)
		{
			_listingGroupRepository = listingGroupRepository;
		}

		public Task<PagedResult<ListingGroup>> GetFilteredListingGroupsPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter)
		{
			throw new NotImplementedException();
		}

		public async Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId)
		{
			return await _listingGroupRepository.GetListingGroupByIdAsync(listingGroupId);
		}

		public async Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserIdAsync(Guid sellerUserId)
		{
			return await _listingGroupRepository.GetListingGroupsBySellerUserId(sellerUserId);
		}

		public async Task<PagedResult<ListingGroup>> GetListingGroupsPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter = null)
		{
			return await _listingGroupRepository.GetListingGroupsPageAsync(cursor, pageSize, filter);
		}
	}
}
