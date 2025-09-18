using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
	internal class ListingGroupRepository : IListingGroupRepository
	{
		public Task<ListingGroup?> AddListingGroupAsync(ListingGroup listingGroup)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteListingGroup(Guid listingGroupId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserId(Guid sellerUserId)
		{
			throw new NotImplementedException();
		}

		public Task<PagedResult<ListingGroup>> GetListingGroupsPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter)
		{
			throw new NotImplementedException();
		}

		public Task<ListingGroup?> GetListingsGroupByListingIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingGroup?> UpdateListingGroupAsync(ListingGroup listingGroup)
		{
			throw new NotImplementedException();
		}
	}
}
