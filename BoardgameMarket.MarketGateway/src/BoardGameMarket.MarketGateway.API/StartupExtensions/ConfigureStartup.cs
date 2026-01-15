using BoardGameMarket.MarketGateway.API.Configuration;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Services;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using System.Diagnostics;
using System.Text.Json;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases;
using BoardGameMarket.MarketGateway.API.Application.Listings.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Assemblers;

namespace BoardGameMarket.MarketGateway.API.StartupExtensions
{
	public static class ConfigureStartup
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
			jsonOptions.ConfigureJson();

			services.AddControllers().
				AddJsonOptions(options => options.JsonSerializerOptions.ConfigureJson());

			services.AddSingleton(jsonOptions);

			services.AddSingleton<CatalogService>();
			services.AddSingleton<UsersDataService>();

			// Listings microservice
			services.AddScoped<ListingsDataService>();
			services.AddScoped<ListingGroupsDataService>();
			services.AddScoped<QueuesDataService>();
			services.AddScoped<ListingsHttpClient>();

			// Use Cases
			services.AddScoped<GetListingGroupDetailUseCase>();
			services.AddScoped<GetListingGroupSummaryPageUseCase>();

			// Dto assemblers
			services.AddScoped<ListingDetailDtoAssembler>();
			services.AddScoped<ListingGroupSummaryDtoAssembler>();


			services.ConfigureHttpJsonOptions(options => options.SerializerOptions.ConfigureJson());

			services.AddHttpClient<ListingsHttpClient>(config =>
			{
				config.BaseAddress = new Uri($"http://{configuration["LISTINGS_SERVICE_HOST"]}:{configuration["LISTINGS_SERVICE_PORT"]}");
			});



			return services;
		}
	}
}
