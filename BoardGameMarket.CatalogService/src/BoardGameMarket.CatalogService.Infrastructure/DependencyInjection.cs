using BoardGameMarket.CatalogService.Core.Application.AdapterContracts;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.RepositoryContracts;
using BoardGameMarket.CatalogService.Infrastructure.Adapters;
using BoardGameMarket.CatalogService.Infrastructure.Configuration;
using BoardGameMarket.CatalogService.Infrastructure.InMemorySeeders;
using BoardGameMarket.CatalogService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace BoardGameMarket.CatalogService.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInMemoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<ICatalogItemsRepository, CatalogItemsInMemoryRepository>();
			//services.AddSingleton<IBggClient, BggClientInMemory>();
			ConfigureBggClient(services, configuration);

			return services;
		}

		public static IServiceCollection AddSeeding(this IServiceCollection services)
		{
			services.AddTransient<SeedRunner>();
			services.AddTransient<ISeeder, CatalogItemSeeder>();
			return services;
		}

		private static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
		{
            services.Configure<MongoDbConfiguration>(configuration.GetSection("MongoDbConfiguration"));

            BsonClassMap.RegisterClassMap<CatalogItem>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });

            services.AddSingleton<IMongoClient>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;

                string connectionStringTemplate = configuration.GetConnectionString("MongoDB")!;

                string connectionString = connectionStringTemplate
                .Replace("$MONGO_HOST", config.MongoHost)
                .Replace("$MONGO_PORT", config.MongoPort);
                return new MongoClient(connectionString);
            });

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;

                IMongoClient client = sp.GetRequiredService<IMongoClient>();

				return client.GetDatabase(config.MongoDatabase);
            });

			return services;
        }

		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddMongoDb(configuration);
			services.AddScoped<ICatalogItemsRepository, CatalogItemsMongoDbRepository>();

			ConfigureBggClient(services, configuration);

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
