using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Application.Users.Mappers
{
	public static class UserMappers
	{
		public static UserSummaryDto ToUserSummaryDto(this UserInfo userInfo)
		{
			return new UserSummaryDto()
			{
				UserId = userInfo.UserId,
				DisplayName = userInfo.DisplayName,
			};
		}
	}
}
