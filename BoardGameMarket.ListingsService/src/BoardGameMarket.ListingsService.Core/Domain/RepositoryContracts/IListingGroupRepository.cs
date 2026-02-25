using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Pagination;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingGroupRepository
	{
		Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId);
		Task<IEnumerable<ListingGroup>> GetListingGroupsByIds(IEnumerable<Guid> listingGroupIds);
		Task<ListingGroup?> GetListingGroupByListingIdAsync(Guid listingId);
		Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserId(Guid sellerUserId);
		Task<int> AddListingGroupAsync(ListingGroup listingGroup);
		Task<int> UpdateListingGroupAsync(ListingGroup listingGroup);
		Task<int> DeleteListingGroupAsync(Guid listingGroupId);
		Task<PagedResult<ListingGroup>> GetListingGroupsPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter);
		Task<IEnumerable<ListingGroup>> GetFilteredListingGroupsAsync(ListingGroupFilter filter);
	}
}
