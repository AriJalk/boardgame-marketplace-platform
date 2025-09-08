namespace BoardGameMarket.UsersService.Core.Domain.Entities
{
	public class UserRole
	{
		public Guid UserRoleId { get; set; }

		public Guid UserId { get; set; }
		public ApplicationUser User { get; set; } = null!;

		public Guid RoleId { get; set; }
		public ApplicationRole Role { get; set; } = null!;

		public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
	}
}
