using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases
{
	public class GetFilteredListingGroupSummariesUseCase
	{
		private readonly ListingGroupsDataService _listingGroupsService;
		private readonly ListingGroupSummaryDtoAssembler _listingGroupSummaryDtoAssembler;

		public GetFilteredListingGroupSummariesUseCase(ListingGroupsDataService listingGroupsService, ListingGroupSummaryDtoAssembler listingGroupSummaryDtoAssembler)
		{
			_listingGroupsService = listingGroupsService;
			_listingGroupSummaryDtoAssembler = listingGroupSummaryDtoAssembler;
		}

		public async Task<ListingGroupSummaryDto[]> ExecuteAsync(ListingGroupFilter filter)
		{
			ListingGroupSummary[] listingGroups = await _listingGroupsService.GetFilteredListingGroupSummaries(filter);

			List<ListingGroupSummaryDto> listingGroupDtos = new List<ListingGroupSummaryDto>();
			foreach (ListingGroupSummary listingGroup in listingGroups)
			{
				listingGroupDtos.Add(await _listingGroupSummaryDtoAssembler.BuildListingGroupSummaryDto(listingGroup));
			}

			return listingGroupDtos.ToArray();
		}
	}
}
