using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories;
using BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories.Seeders;
using BoardGameMarket.ListingsService.Infrastructure.Repositories;
using BoardGameMarket.ListingsService.Infrastructure.Repositories.DbContext;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BoardGameMarket.ListingsService.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services)
		{
			services.AddScoped<IListingGroupRepository, ListingGroupRepository>();
			services.AddScoped<IListingRepository, ListingRepository>();
			services.AddScoped<IListingItemRepository, ListingItemRepository>();
			services.AddScoped<IListingQueueEntryRepository, ListingQueueEntryRepository>();
			services.AddScoped<IListingSoldRepository, ListingSoldRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}

		public static IServiceCollection AddInMemoryInfrastructure(this IServiceCollection services)
		{
			services.AddTransient<IUnitOfWork, InMemoryUnitOfWork>();
			services.AddSingleton<IListingGroupRepository, InMemoryListingGroupRepository>();
			services.AddSingleton<IListingRepository, InMemoryListingRepository>();
			services.AddSingleton<IListingItemRepository, InMemoryListingItemRepository>();
			services.AddSingleton<IListingQueueEntryRepository, InMemoryListingQueueEntryRepository>();
			services.AddSingleton<IListingSoldRepository, InMemoryListingSoldRepository>();

			return services;
		}

		public static IServiceCollection AddInMemorySeeding(this IServiceCollection services)
		{
			JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
			jsonOptions.PropertyNameCaseInsensitive = true;
			jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
			services.AddSingleton(jsonOptions);

			services.AddTransient<InMemorySeedRunner>();
			services.AddTransient<ISeeder, ListingGroupsSeeder>();
			services.AddTransient<ISeeder, ListingsSeeder>();
			services.AddTransient<ISeeder, ListingItemsSeeder>();
			services.AddTransient<QueueSeeder>();

			return services;
		}
	}
}
