namespace BoardGameMarket.UsersService.Core.Application.Contracts
{
	public record UserInfo(Guid Id, string? FirstName, string? LastName, string? DisplayName) { };
}
