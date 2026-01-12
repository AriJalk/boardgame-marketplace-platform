using TestingService.Domains.Users.Dtos;
using TestingService.Domains.Users.ServiceClients;

namespace TestingService.Domains.Users.Helpers
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
