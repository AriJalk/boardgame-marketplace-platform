using BoardGameMarket.MarketGateway.Core.Application.Users;
using BoardGameMarket.MarketGateway.Infrastructure.Cache;
using Microsoft.Extensions.Caching.Distributed;

namespace BoardGameMarket.MarketGateway.Infrastructure.HttpClients
{
	internal class UsersServiceHttpClient : BaseApiClient, IUsersServiceHttpClient
	{
		public UsersServiceHttpClient(HttpClient httpClient, IDistributedCache cache, ICacheOptionsProvider cacheOptionsProvider) : base(httpClient, cache, cacheOptionsProvider) { }

		public async Task<UserDto?> GetUserByUserIdAsync(Guid userId)
		{
			return await GetFromApiAsync<UserDto>($"api/users/{userId}", $"user:{userId}");
		}

		public async Task<UserDto?> RegisterUserByEntraId(UserDto userDto)
		{
			return await PostToApiAsync<UserDto>($"api/users", userDto);
		}
	}
}
