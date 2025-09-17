using BoardGameMarket.BggCatalogService.Core.Application.AdapterContracts;
using BoardGameMarket.BggCatalogService.Core.Domain.RepositoryContracts;
using BoardGameMarket.BggCatalogService.Infrastructure.Adapters;
using BoardGameMarket.BggCatalogService.Infrastructure.Configuration;
using BoardGameMarket.BggCatalogService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BoardGameMarket.BggCatalogService.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{

			services.Configure<MongoDbConfiguration>(configuration.GetSection("MondoDbConfiguration"));

			services.AddSingleton<IMongoClient>(sp =>
			{
				var config = sp.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;

				var connectionString = $"mongodb://{config.MongoHost}:{config.MongoPort}";
				return new MongoClient(connectionString);
			});

			services.AddSingleton<IBggItemsRepository, BggItemsInMemoryRepository>();
			//services.AddTransient<IBggClient, BggClientInMemory>();

			services.AddSingleton<ICategoriesRepository, CategoriesInMemoryRepository>();

			services.AddHttpClient<IBggClient, BggClient>(client =>
			{
				client.BaseAddress = new Uri("https://boardgamegeek.com/xmlapi/");
				client.Timeout = TimeSpan.FromSeconds(30);
			});


			return services;
		}
	}
}
