using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories;
using BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories.Seeders;
using BoardGameMarket.ListingsService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
			services.AddTransient<InMemorySeedRunner>();
			services.AddTransient<ISeeder, ListingGroupsSeeder>();
			services.AddTransient<ISeeder, ListingsSeeder>();
			services.AddTransient<ISeeder, ListingItemsSeeder>();

			return services;
		}
	}
}
