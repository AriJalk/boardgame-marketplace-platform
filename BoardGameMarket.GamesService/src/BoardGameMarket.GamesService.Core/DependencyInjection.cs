using BoardGameMarket.GamesService.Core.Application.ServiceContracts;
using BoardGameMarket.GamesService.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.GamesService.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCore(this IServiceCollection services)
		{
			services.AddTransient<IBggItemFactory, BggItemFactory>();
			services.AddTransient<IBggItemGetterService, BggItemGetterService>();
			return services;
		}
	}
}
