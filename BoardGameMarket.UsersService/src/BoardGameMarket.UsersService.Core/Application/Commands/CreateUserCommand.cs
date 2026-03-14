namespace BoardGameMarket.UsersService.Core.Application.Commands
{
	public record CreateUserCommand(Guid Id, string? FirstName, string? LastName, string DisplayName, string Email)
	{
	}
}
