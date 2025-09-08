using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Infrastructure.DbContexts;
using BoardGameMarket.UsersService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models.ExternalConnectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
			});

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}
}
