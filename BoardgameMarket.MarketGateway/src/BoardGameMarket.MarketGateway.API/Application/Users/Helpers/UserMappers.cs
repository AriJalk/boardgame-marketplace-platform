using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Application.Users.Helpers
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
