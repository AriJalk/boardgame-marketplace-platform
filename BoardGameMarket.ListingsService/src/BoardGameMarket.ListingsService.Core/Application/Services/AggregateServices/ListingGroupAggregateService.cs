using BoardGameMarket.ListingsService.Core.Application.Common;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Services.AggregateServices
{
	public class ListingGroupAggregateService
	{
		private IListingGetterService _listingGetterService;
		private IListingGroupGetterService _listingGroupGetterService;

		private ListingAggregateService _listingAggregateService;

		public ListingGroupAggregateService(IListingGetterService listingGetterService, IListingGroupGetterService listingGroupGetterService, ListingAggregateService listingAggregateService)
		{
			_listingGetterService = listingGetterService;
			_listingGroupGetterService = listingGroupGetterService;
			_listingAggregateService = listingAggregateService;
		}

		public async Task<ListingGroupAggregate> GetListingGroupAggregateAsync(Guid listingGroupId, bool loadItems, bool loadQueues)
		{
			ListingGroup? listingGroup = await _listingGroupGetterService.GetListingGroupByIdAsync(listingGroupId);

			if (listingGroup == null)
			{
				throw new KeyNotFoundException();
			}

			IReadOnlyCollection<ListingAggregate> listings = (await _listingAggregateService.GetListingAggregatesByListingGroupIdAsync(listingGroupId, loadItems, loadQueues)).ToList().AsReadOnly();

			return new ListingGroupAggregate(listingGroup, listings);
		}

		public async Task<PagedResult<ListingGroupAggregate>> GetSummaryPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter)
		{
			PagedResult<ListingGroup> page = await _listingGroupGetterService.GetListingGroupsPageAsync(cursor, pageSize, filter);

			List<ListingGroupAggregate> listingGroupAggragates = new List<ListingGroupAggregate>();
			foreach (ListingGroup listingGroup in page.Items)
			{
				IEnumerable<ListingAggregate> listingAggregates = await _listingAggregateService.GetListingAggregatesByListingGroupIdAsync(listingGroup.Id, loadItems: false, loadQueue: false);

				listingGroupAggragates.Add(new ListingGroupAggregate(listingGroup, listingAggregates.ToList().AsReadOnly()));
			}
			return new PagedResult<ListingGroupAggregate>() { Items = listingGroupAggragates.ToArray(), NextCursor = page.NextCursor };
		}
	}
}
