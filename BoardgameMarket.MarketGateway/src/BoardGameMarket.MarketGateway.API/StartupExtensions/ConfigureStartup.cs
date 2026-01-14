using BoardGameMarket.MarketGateway.API.Configuration;
using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Services;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Services;
using BoardGameMarket.MarketGateway.API.Modules.Queues.Services;
using BoardGameMarket.MarketGateway.API.Modules.Users.Services;
using System.Diagnostics;
using System.Text.Json;

namespace BoardGameMarket.MarketGateway.API.StartupExtensions
{
	public static class ConfigureStartup
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
			jsonOptions.ConfigureJson();

			services.AddControllers().
				AddJsonOptions(options => options.JsonSerializerOptions.ConfigureJson());

			services.AddSingleton(jsonOptions);

			services.AddSingleton<CatalogService>();
			services.AddSingleton<UsersService>();

			services.AddScoped<ListingsDataService>();
			services.AddScoped<QueuesDataService>();

			services.AddScoped<ListingsHttpClient>();

			services.ConfigureHttpJsonOptions(options => options.SerializerOptions.ConfigureJson());

			services.AddHttpClient<ListingsHttpClient>(config =>
			{
				config.BaseAddress = new Uri($"http://{configuration["LISTINGS_SERVICE_HOST"]}:{configuration["LISTINGS_SERVICE_PORT"]}");
			});



			return services;
		}
	}
}
