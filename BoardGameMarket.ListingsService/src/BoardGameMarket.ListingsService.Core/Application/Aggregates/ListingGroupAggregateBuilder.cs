using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Pagination;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Aggregates
{
	public class ListingGroupAggregateBuilder
	{
		private IListingReadService _listingReadService;
		private IListingGroupReadService _listingGroupReadService;

		private ListingAggregateBuilder _listingAggregateBuilder;

		private ILogger<ListingGroupAggregateBuilder> _logger;

		public ListingGroupAggregateBuilder(IListingReadService listingReadService, IListingGroupReadService listingGroupReadService, ListingAggregateBuilder listingAggregateBuilder, ILogger<ListingGroupAggregateBuilder> logger)
		{
			_listingReadService = listingReadService;
			_listingGroupReadService = listingGroupReadService;
			_listingAggregateBuilder = listingAggregateBuilder;
			_logger = logger;
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
			if (pageSize <= 0)
			{
				_logger.LogWarning("PageSize must be larger than 0");
				return new PagedResult<ListingGroupAggregate>();
			}
			if (filter?.ListingIds != null && filter.ListingIds.Any())
			{
				List<Listing> listings = (await _listingReadService.GetListingsByIdsAsync(filter.ListingIds)).ToList();
				if (listings.Count < filter.ListingIds.Count())
				{
					_logger.LogInformation("ListingIds contained non-existant entries");
					return new PagedResult<ListingGroupAggregate>();
				}

				filter.ListingGroupIds = (filter.ListingGroupIds ?? Enumerable.Empty<Guid>())
					.Concat(listings.Select(l => l.ListingGroupId)).ToList();
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
