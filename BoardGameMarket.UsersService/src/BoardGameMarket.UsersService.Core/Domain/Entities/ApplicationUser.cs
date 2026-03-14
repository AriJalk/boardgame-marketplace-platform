
namespace BoardGameMarket.UsersService.Core.Domain.Entities
{
	public class ApplicationUser
	{
		public Guid Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string DisplayName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
	}
}
