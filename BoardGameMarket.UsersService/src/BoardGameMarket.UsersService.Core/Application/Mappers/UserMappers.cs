
using BoardGameMarket.UsersService.Core.Application.DTOs;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using Microsoft.Graph.Models;

namespace BoardGameMarket.UsersService.Core.Application.Mappers
{
	public static class UserMappers
	{
		public static UserDto ToDto(this ApplicationUser user)
		{
			return new UserDto(user.UserId, user.Role, user.FirstName, user.LastName, user.DisplayName);
		}

		public static ApplicationUser ToApplicationUser(this UserDto userDto)
		{
			return new ApplicationUser()
			{
				UserId = userDto.UserId,
				DisplayName = userDto.DisplayName,
				FirstName = userDto.FirstName,
				LastName = userDto.LastName,
				Role = userDto.Roles,
			};
		}
	}
}
