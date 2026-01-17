using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Application.Services.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Application.UseCases;
using BoardGameMarket.UsersService.Core.Domain.Services;
using Microsoft.Extensions.DependencyInjection;


namespace BoardGameMarket.UsersService.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCoreServices(this IServiceCollection services)
		{
			services.AddScoped<IUserGetterService, UserGetterService>();
			services.AddScoped<IUserDeleterService, UserDeleterService>();
			services.AddScoped<IUserAdderService, UserAdderService>();

			services.AddScoped<CreateUserUseCase>();

			services.AddSingleton<UserUpdateDomainService>();

			return services;
		}
	}
}
