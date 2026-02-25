using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Pagination;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
    internal class ListingGroupRepository : IListingGroupRepository
    {
        public Task<int> AddListingGroupAsync(ListingGroup listingGroup)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteListingGroupAsync(Guid listingGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingGroup>> GetFilteredListingGroupsAsync(ListingGroupFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<ListingGroup?> GetListingGroupByListingIdAsync(Guid listingId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingGroup>> GetListingGroupsByIds(IEnumerable<Guid> listingGroupIds)
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

        public Task<int> UpdateListingGroupAsync(ListingGroup listingGroup)
        {
            throw new NotImplementedException();
        }
    }
}
