namespace BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients
{
    public record CreateUserRequest(Guid Id, string? FirstName, string? LastName, string DisplayName, string Email)
    {
    }
}
