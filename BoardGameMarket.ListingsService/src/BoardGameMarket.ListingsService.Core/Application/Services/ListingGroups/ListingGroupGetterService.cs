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
	public class ListingGroupGetterService : IListingGroupGetterService
	{
		private readonly IListingGroupRepository _listingGroupRepository;
		private readonly ILogger<ListingGroupGetterService> _logger;

		public ListingGroupGetterService(IListingGroupRepository listingGroupRepository, ILogger<ListingGroupGetterService> logger)
		{
			_listingGroupRepository = listingGroupRepository;
			_logger = logger;
		}

		public Task<PagedResult<ListingGroup>> GetFilteredListingGroupsPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter)
		{
			throw new NotImplementedException();
		}

		public async Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId)
		{
			_logger.LogInformation($"{nameof(ListingGroupGetterService)}:{nameof(GetListingGroupByIdAsync)}- get group {listingGroupId}");

			return await _listingGroupRepository.GetListingGroupByIdAsync(listingGroupId);
		}

		public async Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserIdAsync(Guid sellerUserId)
		{
			_logger.LogInformation($"{nameof(ListingGroupGetterService)}:{nameof(GetListingGroupsBySellerUserIdAsync)}- get group by seller id {sellerUserId}");

			return await _listingGroupRepository.GetListingGroupsBySellerUserId(sellerUserId);
		}

		public async Task<PagedResult<ListingGroup>> GetListingGroupsPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter = null)
		{
			return await _listingGroupRepository.GetListingGroupsPageAsync(cursor, pageSize, filter);
		}
	}
}
