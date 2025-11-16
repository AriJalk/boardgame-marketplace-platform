namespace BoardGameMarket.MarketGateway.Core.Application.Users
{
	public interface IUsersServiceHttpClient
	{
		Task<UserDto?> GetUserByUserIdAsync(Guid userId);
		Task<UserDto?> RegisterUserByEntraId(UserDto userDto);
	}
}
