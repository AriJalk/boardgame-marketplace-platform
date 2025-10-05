namespace BoardGameMarket.UsersService.Core.Domain.Enums
{
	[Flags]
	public enum RoleType
	{
		User = 1,
		Moderator = 2,
		Admin = 3,
	}

	public static class RolesHelper
	{
		public static RoleType AddRoles(this RoleType initialRoles, params RoleType[] rolesToAdd)
		{
			foreach (RoleType roleToAdd in rolesToAdd)
			{
				initialRoles |= roleToAdd;
			}
			return initialRoles;
		}

		public static RoleType RemoveRoles(this RoleType initialRoles, params RoleType[] rolesToRemove)
		{
			foreach (RoleType roleToRemove in rolesToRemove)
			{
				initialRoles &= ~roleToRemove;
			}
			return initialRoles;
		}

		public static bool IsAuthorized(this RoleType userRoles, RoleType validRoles)
		{
			return (userRoles & validRoles) != 0;
		}
	}
}
