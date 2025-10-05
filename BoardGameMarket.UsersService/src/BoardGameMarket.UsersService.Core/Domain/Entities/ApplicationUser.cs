using BoardGameMarket.UsersService.Core.Domain.Enums;

namespace BoardGameMarket.UsersService.Core.Domain.Entities
{
	public class ApplicationUser
	{
		public Guid UserId { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? DisplayName { get; set; }

		public RoleType Role { get; set; }

		public bool HasRole(RoleType roleType)
		{
			return Role.HasFlag(roleType);
		}

		public bool HasAnyRole(RoleType rolesToCheck)
		{
			return (Role & rolesToCheck) != 0;
		}

	}
}
