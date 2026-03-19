using BoardGameMarket.ListingsService.Core.Application.Pagination;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories
{
    internal class InMemoryListingGroupRepository : IListingGroupRepository
    {
        private readonly List<ListingGroup> _listingGroups = new List<ListingGroup>();

        private PagedResult<ListingGroup> GetGroupsPage(List<ListingGroup> orderedListingGroups, Cursor? cursor, int pageSize)
        {
            int start = 0;
            if (cursor != null)
            {
                start = orderedListingGroups.FindIndex(g =>
                    g.CreatedAt < cursor.LastCreatedAt || (g.CreatedAt == cursor.LastCreatedAt && g.Id.CompareTo(cursor.LastId) < 0));
                if (start < 0)
                {
                    start = orderedListingGroups.Count;
                }
            }

            var pageItems = orderedListingGroups.Skip(start).Take(pageSize + 1).ToList();

            bool hasNext = pageItems.Count > pageSize;

            string? nextCursor = null;

            if (hasNext)
            {
                pageItems.RemoveAt(pageItems.Count - 1);
                var last = pageItems.Last();
                nextCursor = $"{last.CreatedAt:o}|{last.Id}";
                nextCursor = Convert.ToBase64String(Encoding.UTF8.GetBytes(nextCursor));
            }
            return new PagedResult<ListingGroup>
            {
                Items = pageItems.ToArray(),
                NextCursor = nextCursor
            };
        }

        private IEnumerable<ListingGroup> QueryListingGroup(IEnumerable<ListingGroup> query, ListingGroupFilter filter)
        {
            if (filter.SellerUserId is Guid sellerUserId)
            {
                query = query.Where(g => g.SellerUserId == sellerUserId);
            }

            if (!string.IsNullOrEmpty(filter.Location))
            {
                query = query.Where(g => g.Location == filter.Location);
            }

            if (filter.ListingGroupIds != null)
            {
                IEnumerable<Guid> distinctIds = filter.ListingGroupIds.Distinct();
                query = query.Where(g => distinctIds.Contains(g.Id));
            }

            return query;
        }

        public Task<int> AddListingGroupAsync(ListingGroup listingGroup)
        {
            _listingGroups.Add(listingGroup);
            return Task.FromResult(1);
        }

        public Task<int> DeleteListingGroupAsync(Guid listingGroupId)
        {
            int index = _listingGroups.FindIndex(lg => lg.Id == listingGroupId);
            if (index == -1)
            {
                return Task.FromResult(0);
            }

            _listingGroups.RemoveAt(index);
            return Task.FromResult(1);
        }

        public Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId)
        {
            return Task.FromResult(_listingGroups.FirstOrDefault(l => l.Id == listingGroupId));
        }

        public Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserId(Guid sellerUserId)
        {
            return Task.FromResult(_listingGroups.Where(l => l.SellerUserId == sellerUserId));
        }

        public Task<PagedResult<ListingGroup>> GetListingGroupsPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter)
        {
            IEnumerable<ListingGroup> query = _listingGroups;
            if (filter != null)
            {
                query = QueryListingGroup(query, filter);
            }

            List<ListingGroup> ordered = query.OrderByDescending(g => g.CreatedAt).ThenByDescending(g => g.Id).ToList();
            return Task.FromResult(GetGroupsPage(ordered, cursor, pageSize));
        }

        public Task<ListingGroup?> GetListingGroupByListingIdAsync(Guid listingId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateListingGroupAsync(ListingGroup listingGroup)
        {
            int index = _listingGroups.FindIndex(lg => lg.Id == listingGroup.Id);
            if (index == -1)
            {
                return Task.FromResult(0);
            }
            _listingGroups[index] = listingGroup;
            return Task.FromResult(1);
        }

        public Task<IEnumerable<ListingGroup>> GetListingGroupsByIds(IEnumerable<Guid> listingGroupIds)
        {
            return Task.FromResult(_listingGroups.Where(lg => listingGroupIds.Contains(lg.Id)));
        }

        public Task<IEnumerable<ListingGroup>> GetFilteredListingGroupsAsync(ListingGroupFilter filter)
        {
            IEnumerable<ListingGroup> query = QueryListingGroup(_listingGroups, filter);
            List<ListingGroup> ordered = query.OrderByDescending(g => g.CreatedAt).ThenByDescending(g => g.Id).ToList();
            return Task.FromResult(query);
        }
    }
}
