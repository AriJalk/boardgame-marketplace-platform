using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Pagination;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Aggregates
{
	public class ListingGroupAggregateBuilder
	{
		private IListingReadService _listingReadService;
		private IListingGroupReadService _listingGroupReadService;

		private ListingAggregateBuilder _listingAggregateBuilder;

		public ListingGroupAggregateBuilder(IListingReadService listingReadService, IListingGroupReadService listingGroupReadService, ListingAggregateBuilder listingAggregateBuilder)
		{
			_listingReadService = listingReadService;
			_listingGroupReadService = listingGroupReadService;
			_listingAggregateBuilder = listingAggregateBuilder;
		}

		public async Task<ListingGroupAggregate> GetListingGroupAggregateAsync(Guid listingGroupId, bool loadItems, bool loadQueues)
		{
			ListingGroup? listingGroup = await _listingGroupReadService.GetListingGroupByIdAsync(listingGroupId);

			if (listingGroup == null)
			{
				throw new KeyNotFoundException($"Listing group {listingGroupId} not found");
			}

			IReadOnlyCollection<ListingAggregate> listings = (await _listingAggregateBuilder.GetListingAggregatesByListingGroupIdAsync(listingGroupId, loadItems, loadQueues)).ToList().AsReadOnly();

			return new ListingGroupAggregate(listingGroup, listings);
		}

		public async Task<PagedResult<ListingGroupAggregate>> GetSummaryPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter)
		{
			if (filter?.ListingIds != null && filter.ListingIds.Any())
			{
				IEnumerable<Guid> listingGroupIdsFromListings = await _listingReadService
					.GetListingsByIdsAsync(filter.ListingIds)
					.ContinueWith(t => t.Result.Select(l => l.ListingGroupId));
				filter.ListingGroupIds = (filter.ListingGroupIds ?? Enumerable.Empty<Guid>())
					.Concat(listingGroupIdsFromListings)
					.Distinct();
			}
			PagedResult<ListingGroup> page = await _listingGroupReadService.GetListingGroupsPageAsync(cursor, pageSize, filter);

			ListingGroupAggregate[] listingGroupAggregates = await Task.WhenAll(page.Items.Select(async lg =>
			{
				IEnumerable<ListingAggregate> listingAggregates = await _listingAggregateBuilder.GetListingAggregatesByListingGroupIdAsync(lg.Id, loadItems: false, loadQueue: false);

				return new ListingGroupAggregate(lg, listingAggregates.ToArray());
			}));

			return new PagedResult<ListingGroupAggregate>() { Items = listingGroupAggregates, NextCursor = page.NextCursor };
		}

		public async Task<IReadOnlyCollection<ListingGroupAggregate>> GetFilteredListingGroups(ListingGroupFilter filter)
		{
			if (filter.ListingIds != null && filter.ListingIds.Any())
			{
				List<Guid> listingGroupIdsFromListings = (await _listingReadService
					.GetListingsByIdsAsync(filter.ListingIds)).Select(l => l.ListingGroupId).ToList();
				filter.ListingGroupIds = (filter.ListingGroupIds ?? Enumerable.Empty<Guid>())
					.Concat(listingGroupIdsFromListings)
					.Distinct().ToList();
			}

			 IEnumerable<ListingGroup> filteredListingGroups = await _listingGroupReadService.GetFilteredListingGroups(filter);

			ListingGroupAggregate[] listingGroupAggregates = await Task.WhenAll(filteredListingGroups.Select(async lg => 
			{
				IEnumerable<ListingAggregate> listingAggregates = await _listingAggregateBuilder.GetListingAggregatesByListingGroupIdAsync(lg.Id, loadItems: false, loadQueue: false);

				return new ListingGroupAggregate(lg, listingAggregates.ToArray());
			}));

			return listingGroupAggregates;
		}
	}
}
