using BoardGameMarket.CatalogService.Core.Application.AdapterContracts;
using BoardGameMarket.CatalogService.Core.Domain.RepositoryContracts;
using BoardGameMarket.CatalogService.Infrastructure.Adapters;
using BoardGameMarket.CatalogService.Infrastructure.Configuration;
using BoardGameMarket.CatalogService.Infrastructure.InMemorySeeders;
using BoardGameMarket.CatalogService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BoardGameMarket.CatalogService.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInMemoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<ICatalogItemsRepository, CatalogItemsInMemoryRepository>();

			return services;
		}

		public static IServiceCollection AddInMemorySeeding(this IServiceCollection services)
		{
			services.AddTransient<InMemorySeedRunner>();
			services.AddTransient<ISeeder, CatalogItemSeeder>();
			return services;
		}

		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{

			services.Configure<MongoDbConfiguration>(configuration.GetSection("MondoDbConfiguration"));

			services.AddSingleton<IMongoClient>(sp =>
			{
				var config = sp.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;

				var connectionString = $"mongodb://{config.MongoHost}:{config.MongoPort}";
				return new MongoClient(connectionString);
			});

			services.AddScoped<ICatalogItemsRepository, CatalogItemsMongoDbRepository>();

			return services;
		}

		public static IServiceCollection ConfigureBggClient(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient<IBggClient, BggClient>(client =>
			{
				client.BaseAddress = new Uri("https://boardgamegeek.com/xmlapi/");
				client.Timeout = TimeSpan.FromSeconds(30);
			});

			services.Configure<BggClientConfiguration>(configuration.GetSection("BggClient"));

			return services;
		}

		public static IServiceCollection ConfigureLocalBggClient(this IServiceCollection services)
		{
			services.AddSingleton<IBggClient, BggClientInMemory>();
			return services;
		}
	}
}
