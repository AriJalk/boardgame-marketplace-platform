
using BoardGameMarket.UsersService.Core.Application.DTOs;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using Microsoft.Graph.Models;

namespace BoardGameMarket.UsersService.Core.Application.Mappers
{
	public static class UserMappers
	{
		public static UserDTO ToDTO(this ApplicationUser user)
		{
			IEnumerable<string> roles = user.UserRoles.Select(r => r.Role.Name);
			return new UserDTO(user.UserId, roles, user.FirstName, user.LastName, user.DisplayName);
		}

		public static ApplicationUser ToApplicationUser(this User user)
		{

			if (!Guid.TryParse(user.Id, out Guid entraId))
			{
				throw new InvalidOperationException($"Invalid Entra user ID: {user.Id}");
			}

			return new ApplicationUser
			{
				UserId = entraId,
				DisplayName = user.DisplayName,
				FirstName = user.GivenName,
				LastName = user.Surname
			};
		}
	}
}
