using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases
{
    public class AddListingGroupUseCase
    {
        private readonly ListingGroupsDataService _listingGroupDataService;
        private readonly ListingDetailDtoAssembler _listingDetailDtoAssembler;
        private readonly UsersDataService _usersDataService;

        public AddListingGroupUseCase(ListingGroupsDataService listingGroupDataService, ListingDetailDtoAssembler listingDetailDtoAssembler, UsersDataService usersDataService)
        {
            _listingGroupDataService = listingGroupDataService;
            _listingDetailDtoAssembler = listingDetailDtoAssembler;
            _usersDataService = usersDataService;
        }

        public async Task<ListingGroupDetailDto> ExecuteAsync(CreateListingGroupRequest request)
        {
           ListingGroupDetail listingGroup = await _listingGroupDataService.AddListingGroup(request);

            IEnumerable<Task<ListingDetailDto>> listingTasks = listingGroup.Listings.Select(async listing => {
                return await _listingDetailDtoAssembler.BuildListingDetailDto(listing);
            });
            Task<UserInfo> sellerTask = _usersDataService.GetUser(listingGroup.SellerUserId);

            ListingDetailDto[] listings = await Task.WhenAll(listingTasks);
            UserInfo seller = await sellerTask;

            return listingGroup.ToListingGroupDetailDto(listings, seller.ToUserSummaryDto());
        }
    }
}
