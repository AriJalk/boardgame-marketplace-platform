using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingGroups
{
	public class GetListingGroupsPageUseCase
	{
		ILogger<GetListingGroupsPageUseCase> _logger;
		private readonly IListingGroupGetterService _listingGroupGetterService;
		private readonly ListingGroupDtoComposer _listingGroupDtoComposer;
		private readonly ListingFetcher _listingFetcher;

		public GetListingGroupsPageUseCase(ILogger<GetListingGroupsPageUseCase> logger, IListingGroupGetterService listingGroupGetterService, ListingGroupDtoComposer listingGroupDtoComposer, ListingFetcher listingFetcher)
		{
			_logger = logger;
			_listingGroupGetterService = listingGroupGetterService;
			_listingGroupDtoComposer = listingGroupDtoComposer;
			_listingFetcher = listingFetcher;
		}

		public async Task<PagedResult<ListingGroupResponseDto>> ExecuteAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter)
		{
			if (filter != null && filter.CatalogId.HasValue)
			{
				HashSet<Guid> groupIds = await _listingFetcher.GetListingGroupIdsByCatalogId(filter.CatalogId.Value);
				filter.ListingGroupIds = groupIds;
			}
			PagedResult<ListingGroup> pagedResult = await _listingGroupGetterService.GetListingGroupsPageAsync(cursor, pageSize, filter);

			IEnumerable<ListingGroupResponseDto> dtoResponses = await Task.WhenAll(pagedResult.Items.Select(_listingGroupDtoComposer.ComposeListingGroupAsync));
			return new PagedResult<ListingGroupResponseDto>()
			{
				Items = dtoResponses.ToArray(),
				NextCursor = pagedResult.NextCursor
			};
		}
	}
}
