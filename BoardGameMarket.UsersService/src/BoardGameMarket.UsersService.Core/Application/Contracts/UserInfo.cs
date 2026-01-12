using BoardGameMarket.UsersService.Core.Domain.Enums;

namespace BoardGameMarket.UsersService.Core.Application.Contracts
{
	public record UserInfo(Guid UserId, RoleType Role, string? FirstName, string? LastName, string? DisplayName) { };
}
