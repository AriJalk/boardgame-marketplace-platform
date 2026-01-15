using BoardGameMarket.MarketGateway.API.Application.Users.Enums;

namespace BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients
{
	public record UserInfo(Guid Id, RoleType Role, string? FirstName, string? LastName, string? DisplayName) { };
}
