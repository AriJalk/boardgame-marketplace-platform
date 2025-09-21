using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories;
using BoardGameMarket.ListingsService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			services.AddSingleton<IListingGroupRepository, InMemoryRepository>();
			services.AddSingleton<IListingRepository, InMemoryRepository>();
			services.AddSingleton<IListingItemRepository, InMemoryRepository>();
			services.AddSingleton<IListingQueueEntryRepository, InMemoryRepository>();
			services.AddSingleton<IListingSoldRepository, InMemoryRepository>();
			return services;
		}
	}
}
