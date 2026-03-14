using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Users.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Assemblers
{
	public class ListingGroupSummaryDtoAssembler
	{
		private readonly UsersDataService _usersService;

		public ListingGroupSummaryDtoAssembler(UsersDataService usersService)
		{
			_usersService = usersService;
		}

		public async Task<ListingGroupSummaryDto> BuildListingGroupSummaryDto(ListingGroupSummary listingGroup)
		{
			UserInfo seller = await _usersService.GetUserOrThrow(listingGroup.SellerUserId);
			UserSummaryDto sellerDto = seller.ToUserSummaryDto();
			ListingSummaryDto[] listings = listingGroup.Listings.Select(l => l.ToListingSummaryDto(sellerDto)).ToArray();
			return listingGroup.ToListingGroupSummaryDto(listings, sellerDto);
		}
	}
}
