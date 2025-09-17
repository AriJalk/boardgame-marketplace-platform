using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingGroups
{
	public class GetListingGroupsPageUseCase
	{
		private readonly IListingGroupGetterService _listingGroupGetterService;
		private readonly ListingGroupDtoComposer _listingGroupDtoComposer;

		public GetListingGroupsPageUseCase(IListingGroupGetterService listingGroupGetterService, ListingGroupDtoComposer listingGroupDtoComposer)
		{
			_listingGroupGetterService = listingGroupGetterService;
			_listingGroupDtoComposer = listingGroupDtoComposer;
		}

		public async Task<PagedResult<ListingGroupResponseDto>> ExecuteAsync(Cursor? cursor, int pageSize)
		{

			PagedResult<ListingGroup> pagedResult = await _listingGroupGetterService.GetListingGroupsPageAsync(cursor, pageSize);
			IEnumerable<ListingGroupResponseDto> dtoResponses = await Task.WhenAll(pagedResult.Items.Select(_listingGroupDtoComposer.ComposeListingGroupAsync));
			return new PagedResult<ListingGroupResponseDto>()
			{
				Items = dtoResponses,
				NextCursor = pagedResult.NextCursor
			};
		}
	}
}
