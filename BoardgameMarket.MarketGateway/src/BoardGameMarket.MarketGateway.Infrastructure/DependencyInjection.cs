using BoardGameMarket.MarketGateway.Core.Application.Users;
using BoardGameMarket.MarketGateway.Infrastructure.Cache;
using BoardGameMarket.MarketGateway.Infrastructure.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoardGameMarket.MarketGateway.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient<IUsersServiceHttpClient, UsersServiceHttpClient>(config =>
			{
				config.BaseAddress = new Uri($"http://{configuration["USERS_MICROSERVICE_HOST"]}:{configuration["USERS_MICROSERVICE_PORT"]}");
			});

			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = $"{configuration["REDIS_HOST"]}:{configuration["REDIS_PORT"]}";
			});

			services.AddSingleton<ICacheOptionsProvider, CacheOptionsProvider>();

			return services;
		}
	}
}
