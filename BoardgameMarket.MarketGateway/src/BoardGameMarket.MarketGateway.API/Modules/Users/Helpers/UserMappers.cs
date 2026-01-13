using BoardGameMarket.MarketGateway.API.Modules.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Modules.Users.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Modules.Users.Helpers
{
	public static class UserMappers
	{
		public static UserSummaryDto ToUserSummaryDto(this UserInfo userInfo)
		{
			return new UserSummaryDto()
			{
				Id = userInfo.Id,
				DisplayName = userInfo.DisplayName,
			};
		}
	}
}
