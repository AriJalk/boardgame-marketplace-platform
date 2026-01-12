using TestingService.Domains.Users.Enums;

namespace TestingService.Domains.Users.ServiceClients
{
	public record UserInfo(Guid Id, RoleType Role, string? FirstName, string? LastName, string? DisplayName) { };
}
