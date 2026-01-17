namespace BoardGameMarket.UsersService.Core.Application.Contracts
{
	public record UpdateUserRequest(string? FirstName, string? LastName, string? DisplayName) { }
}
