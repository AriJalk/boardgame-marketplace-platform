using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Users.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Assemblers
{
	public class ListingQueueEntryDtoAssembler
	{
		private readonly UsersDataService _usersService;

		public ListingQueueEntryDtoAssembler(UsersDataService usersService)
		{
			_usersService = usersService;
		}

		public async Task<ListingQueueEntryResponseDto> BuildQueueEntryResponseDto(ListingQueueEntryInfo queueEntry)
		{
			UserInfo userInfo = await _usersService.GetUser(queueEntry.BuyerUserId);
			UserSummaryDto userSummaryDto = userInfo.ToUserSummaryDto();

			return queueEntry.ToListingQueueEntryResponseDto(userSummaryDto);
		}
	}
}
