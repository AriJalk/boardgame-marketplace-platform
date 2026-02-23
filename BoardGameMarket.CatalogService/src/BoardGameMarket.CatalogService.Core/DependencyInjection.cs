using BoardGameMarket.CatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.CatalogService.Core.Application.Services;
using BoardGameMarket.CatalogService.Core.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.CatalogService.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCore(this IServiceCollection services)
		{
			services.AddScoped<ICatalogItemGetterService, CatalogItemGetterService>();
			services.AddScoped<ICatalogItemAdderService, CatalogItemAdderService>();

			services.AddScoped<GetBggItemUseCase>();
			services.AddScoped<GetCustomItemUseCase>();
			services.AddScoped<GetCustomItemsUseCase>();

			return services;
		}
	}
}
