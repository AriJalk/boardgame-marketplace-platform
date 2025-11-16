namespace BoardGameMarket.MarketGateway.Core.Application.Users
{
	public class UserDto
	{
		public Guid UserId { get; set; }
		public RoleType Role {  get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? DisplayName { get; set; }

		public UserDto(Guid userId, RoleType role, string? firstName, string? lastName, string? displayName)
		{
			UserId = userId;
			Role = role;
			FirstName = firstName;
			LastName = lastName;
			DisplayName = displayName;
		}

		public UserDto() { }
	}
}
