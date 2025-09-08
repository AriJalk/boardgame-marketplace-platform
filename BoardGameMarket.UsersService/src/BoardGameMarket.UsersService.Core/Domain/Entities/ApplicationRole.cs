using BoardGameMarket.UsersService.Core.Domain.Enums;

namespace BoardGameMarket.UsersService.Core.Domain.Entities
{
	public class ApplicationRole
	{
		public Guid RoleId { get; set; }
		public RoleType RoleType { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }

		public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
	}
}
