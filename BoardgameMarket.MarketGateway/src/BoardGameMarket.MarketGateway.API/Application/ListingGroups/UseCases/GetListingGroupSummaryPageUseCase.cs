using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases
{
	public class GetListingGroupSummaryPageUseCase
	{
		private readonly ListingGroupsDataService _listingGroupsService;
		private readonly ListingGroupSummaryDtoAssembler _listingGroupSummaryDtoAssembler;
		private readonly UsersDataService _usersService;

		public GetListingGroupSummaryPageUseCase(ListingGroupsDataService listingGroupsService, ListingGroupSummaryDtoAssembler listingGroupSummaryDtoAssembler, UsersDataService usersService)
		{
			_listingGroupsService = listingGroupsService;
			_listingGroupSummaryDtoAssembler = listingGroupSummaryDtoAssembler;
			_usersService = usersService;
		}

		public async Task<PagedResult<ListingGroupSummaryDto>> ExecuteAsync(PagedListingGroupQuery query)
		{
			PagedResult<ListingGroupSummary> listingGroupPagedResult = await _listingGroupsService.GetListingGroupsSummaryPage(query);

			List<ListingGroupSummaryDto> listingGroupDtos = new List<ListingGroupSummaryDto>();
			foreach (ListingGroupSummary listingGroup in listingGroupPagedResult.Items)
			{
				listingGroupDtos.Add(await _listingGroupSummaryDtoAssembler.BuildListingGroupSummaryDto(listingGroup));
			}

			return new PagedResult<ListingGroupSummaryDto>()
			{
				Items = listingGroupDtos.ToArray(),
				NextCursor = listingGroupPagedResult.NextCursor
			};
		}
	}
}
