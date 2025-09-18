 using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingGroups
{
	public class GetListingGroupsPageUseCase
	{
		private readonly IListingGroupGetterService _listingGroupGetterService;
		private readonly ListingGroupDtoComposer _listingGroupDtoComposer;
		private readonly ListingFetcher _listingFetcher;

		public GetListingGroupsPageUseCase(IListingGroupGetterService listingGroupGetterService, ListingGroupDtoComposer listingGroupDtoComposer, ListingFetcher listingFetcher)
		{
			_listingGroupGetterService = listingGroupGetterService;
			_listingGroupDtoComposer = listingGroupDtoComposer;
			_listingFetcher = listingFetcher;
		}

		public async Task<PagedResult<ListingGroupResponseDto>> ExecuteAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter)
		{
			if (filter != null && filter.CatalogId.HasValue)
			{
				HashSet<Guid> groupIds = new HashSet<Guid>();
				
				IEnumerable<Guid> listingIds = await _listingFetcher.GetListingIdsByCatalogId(filter.CatalogId.Value);
				IEnumerable<Listing> listings = await _listingFetcher.GetListingsByListingIds(listingIds);
				foreach (Listing listing in listings)
				{
					groupIds.Add(listing.ListingGroupId);
				}
				filter.ListingGroupIds = groupIds;
			}
			PagedResult<ListingGroup> pagedResult = await _listingGroupGetterService.GetListingGroupsPageAsync(cursor, pageSize, filter);
			IEnumerable<ListingGroupResponseDto> dtoResponses = await Task.WhenAll(pagedResult.Items.Select(_listingGroupDtoComposer.ComposeListingGroupAsync));
			return new PagedResult<ListingGroupResponseDto>()
			{
				Items = dtoResponses,
				NextCursor = pagedResult.NextCursor
			};
		}
	}
}
