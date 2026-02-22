using BoardGameMarket.UsersService.Core.Domain.Enums;

namespace BoardGameMarket.UsersService.Core.Application.Commands
{
	public record CreateUserCommand(Guid UserId, RoleType Role, string? FirstName, string? LastName, string DisplayName, string Email)
	{
	}
}
