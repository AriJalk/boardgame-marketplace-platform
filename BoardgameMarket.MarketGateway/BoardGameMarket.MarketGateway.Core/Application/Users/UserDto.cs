namespace BoardGameMarket.MarketGateway.Core.Application.Users
{
	public record UserDto(Guid UserId, RoleType Role, string? FirstName, string? LastName, string? DisplayName) { };
}
