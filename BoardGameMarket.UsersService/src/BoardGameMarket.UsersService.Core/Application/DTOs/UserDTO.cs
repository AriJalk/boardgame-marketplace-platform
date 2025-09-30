using BoardGameMarket.UsersService.Core.Domain.Enums;

namespace BoardGameMarket.UsersService.Core.Application.DTOs
{
	public record UserDto(Guid UserId, RoleType Roles, string? FirstName, string? LastName, string? DisplayName) { };
}
