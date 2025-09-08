namespace BoardGameMarket.UsersService.Core.Domain.Entities
{
	public class ApplicationUser
	{
		public Guid UserId { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? DisplayName { get; set; }

		public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

		public bool HasRole(string roleName)
		{
			return UserRoles.Any(r => r.Role.Name == roleName);
		}
	}
}
