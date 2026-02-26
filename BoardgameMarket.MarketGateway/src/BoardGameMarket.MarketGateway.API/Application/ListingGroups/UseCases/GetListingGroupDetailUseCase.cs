using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Users.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases
{
	public class GetListingGroupDetailUseCase
	{
		private readonly ListingGroupsDataService _listingGroupsService;
		private readonly UsersDataService _usersService;
		private readonly ListingDetailDtoAssembler _listingDtoAssembler;

		public GetListingGroupDetailUseCase(ListingGroupsDataService listingGroupsService, UsersDataService usersService, ListingDetailDtoAssembler listingDtoAssembler)
		{
			_listingGroupsService = listingGroupsService;
			_usersService = usersService;
			_listingDtoAssembler = listingDtoAssembler;
		}

		public async Task<ListingGroupDetailDto> Execute(Guid listingGroupId)
		{
			ListingGroupDetail listingGroup = await _listingGroupsService.GetListingGroupDetail(listingGroupId);
            UserInfo seller = await _usersService.GetUser(listingGroup.SellerUserId);
			UserSummaryDto sellerDto = seller.ToUserSummaryDto();
            var listingTasks = listingGroup.Listings.Select(async listing => { 
				return await _listingDtoAssembler.BuildListingDetailDto(listing, sellerDto);
			});
			

			ListingDetailDto[] listings = await Task.WhenAll(listingTasks);

			return listingGroup.ToListingGroupDetailDto(listings, sellerDto);
		}
	}
}
