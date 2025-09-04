using BoardGameMarket.UsersMicroservice.Core.Domain.Enums;

namespace BoardGameMarket.UsersMicroservice.Core.Domain.Entities
{
	public class ApplicationUser
	{
		public Guid UserId { get; set; }
		public UserRole Role { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? DisplayName { get; set; }

		public bool HasRole(UserRole role)
		{
			return Role == role;
		}
	}
}
