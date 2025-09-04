using BoardGameMarket.UsersMicroservice.Core.Application.ServiceContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts;
using BoardGameMarket.UsersService.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;


namespace BoardGameMarket.UsersService.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCoreServices(this IServiceCollection services)
		{
			services.AddScoped<IUserGetterService, UserGetterService>();
			services.AddScoped<IUserDeleterService, UserDeleterService>();


			return services;
		}
	}
}
