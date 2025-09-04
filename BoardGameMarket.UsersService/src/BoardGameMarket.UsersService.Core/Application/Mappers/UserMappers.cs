using BoardGameMarket.UsersMicroservice.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Application.DTOs;

namespace BoardGameMarket.UsersService.Core.Application.Mappers
{
	public static class UserMappers
	{
		public static UserDTO ToDTO(this ApplicationUser user)
		{
			return new UserDTO(user.UserId, user.Role, user.FirstName, user.LastName, user.DisplayName);
		}

		public static ApplicationUser ToApplicationUser(this UserDTO user)
		{
			return new ApplicationUser() { UserId = user.UserId, Role = user.Role, FirstName = user.FirstName, LastName = user.LastName, DisplayName = user.DisplayName };
		}
	}
}
