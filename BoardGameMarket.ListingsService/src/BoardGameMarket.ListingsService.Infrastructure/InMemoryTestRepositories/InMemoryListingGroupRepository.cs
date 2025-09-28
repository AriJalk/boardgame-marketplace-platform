using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories
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
					g.CreatedAt < cursor.LastCreatedAt || (g.CreatedAt == cursor.LastCreatedAt && g.Id < cursor.LastId));
				if (start < 0) start = orderedListingGroups.Count;
			}

			var pageItems = orderedListingGroups.Skip(start).Take(pageSize).ToArray();
			string? nextCursor = null;

			if (pageItems.Length == pageSize)
			{
				var last = pageItems.Last();
				nextCursor = $"{last.CreatedAt:o}|{last.Id}";
				nextCursor = Convert.ToBase64String(Encoding.UTF8.GetBytes(nextCursor));
			}
			return new PagedResult<ListingGroup>
			{
				Items = pageItems,
				NextCursor = nextCursor
			};
		}
		public Task<ListingGroup?> AddListingGroupAsync(ListingGroup listingGroup)
		{
			_listingGroups.Add(listingGroup);
			return Task.FromResult(listingGroup);
		}

		public Task<bool> DeleteListingGroup(Guid listingGroupId)
		{
			ListingGroup? listingGroup = _listingGroups.FirstOrDefault(l => l.Id == listingGroupId);
			if (listingGroup == null)
			{
				return Task.FromResult(false);
			}
			_listingGroups.Remove(listingGroup);
			return Task.FromResult(true);
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
				if (filter.SellerUserId.HasValue)
				{
					query = query.Where(g => g.SellerUserId == filter.SellerUserId.Value);
				}

				if (!string.IsNullOrEmpty(filter.Location))
				{
					query = query.Where(g => g.Location == filter.Location);
				}

				if (filter.ListingGroupIds != null)
				{
					query = query.Where(g => filter.ListingGroupIds.Contains(g.Id));
				}
			}

			List<ListingGroup> ordered = query.OrderByDescending(g => g.CreatedAt).ThenByDescending(g => g.Id).ToList();
			return Task.FromResult(GetGroupsPage(ordered, cursor, pageSize));
		}

		public Task<ListingGroup?> GetListingsGroupByListingIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingGroup?> UpdateListingGroupAsync(ListingGroup listingGroup)
		{
			ListingGroup? originalListingGroup = _listingGroups.FirstOrDefault(lg => lg.Id == listingGroup.Id);
			_listingGroups.Remove(originalListingGroup);
			_listingGroups.Add(listingGroup);
			return Task.FromResult(listingGroup);
		}
	}
}
