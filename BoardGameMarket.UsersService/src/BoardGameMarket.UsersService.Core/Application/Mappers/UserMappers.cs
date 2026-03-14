
using BoardGameMarket.UsersService.Core.Application.Contracts;
using BoardGameMarket.UsersService.Core.Domain.Entities;

namespace BoardGameMarket.UsersService.Core.Application.Mappers
{
	public static class UserMappers
	{
		public static UserInfo ToUserInfo(this ApplicationUser user)
		{
			return new UserInfo(user.Id, user.FirstName, user.LastName, user.DisplayName);
		}
	}
}
