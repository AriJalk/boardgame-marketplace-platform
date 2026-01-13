using BoardGameMarket.MarketGateway.API.Modules.Users.Enums;

namespace BoardGameMarket.MarketGateway.API.Modules.Users.ServiceClients
{
	public record UserInfo(Guid Id, RoleType Role, string? FirstName, string? LastName, string? DisplayName) { };
}
