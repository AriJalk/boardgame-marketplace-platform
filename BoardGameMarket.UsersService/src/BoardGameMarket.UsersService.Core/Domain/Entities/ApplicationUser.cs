using BoardGameMarket.UsersService.Core.Domain.Enums;

namespace BoardGameMarket.UsersService.Core.Domain.Entities
{
	public class ApplicationUser
	{
		public Guid Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string DisplayName { get; set; } = string.Empty;

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
