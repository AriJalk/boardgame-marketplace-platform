namespace BoardGameMarket.UsersService.Core.Domain.Enums
{
	[Flags]
	public enum RoleType
	{
		User = 1,
		Moderator = 2,
		Admin = 3,
	}
}
