using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
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
	}
}
