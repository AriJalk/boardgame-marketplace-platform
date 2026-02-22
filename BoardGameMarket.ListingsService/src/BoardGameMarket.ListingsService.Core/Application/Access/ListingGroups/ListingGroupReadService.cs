using BoardGameMarket.ListingsService.Core.Application.Pagination;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups
{
	internal class ListingGroupReadService : IListingGroupReadService
	{
		private readonly IListingGroupRepository _listingGroupRepository;

		public ListingGroupReadService(IListingGroupRepository listingGroupRepository)
		{
			_listingGroupRepository = listingGroupRepository;
		}

		public async Task<IEnumerable<ListingGroup>> GetFilteredListingGroups(ListingGroupFilter filter)
		{
			return await _listingGroupRepository.GetFilteredListingGroupsAsync(filter);
		}


		public async Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId)
		{
			return await _listingGroupRepository.GetListingGroupByIdAsync(listingGroupId);
		}

		public async Task<IEnumerable<ListingGroup>> GetListingGroupsByIdsAsync(IEnumerable<Guid> listingGroupIds)
		{
			return await _listingGroupRepository.GetListingGroupsByIds(listingGroupIds);
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
