using BoardGameMarket.ListingsService.Core.Application.Common;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups
{
	public interface IListingGroupReadService
	{
		Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId);
		Task<IEnumerable<ListingGroup>> GetListingGroupsByIdsAsync(IEnumerable<Guid> listingGroupIds);
		Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserIdAsync(Guid sellerUserId);
		Task<PagedResult<ListingGroup>> GetListingGroupsPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter = null);
		Task<IEnumerable<ListingGroup>> GetFilteredListingGroups(ListingGroupFilter filter);
	}
}
