using BoardGameMarket.MarketGateway.API.Configuration;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Services;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using System.Text.Json;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.UseCases;
using BoardGameMarket.MarketGateway.API.Application.Listings.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.UseCases;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Services;

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

			// Users microservice
			services.AddScoped<UsersDataService>();
			services.AddScoped<LoginService>();

			// Catalog microservice
			services.AddScoped<CatalogDataService>();
			services.AddScoped<CatalogHttpClient>();

			// Listings microservice
			services.AddScoped<ListingsDataService>();
			services.AddScoped<ListingGroupsDataService>();
			services.AddScoped<QueuesDataService>();
			services.AddScoped<ListingsHttpClient>();
			services.AddScoped<ListingReservationDataService>();

			// Use Cases
			services.AddScoped<GetListingGroupDetailUseCase>();
			services.AddScoped<GetListingGroupSummaryPageUseCase>();
			services.AddScoped<GetFilteredListingGroupSummariesUseCase>();

			services.AddScoped<GetListingQueueUseCase>();
			services.AddScoped<GetListingQueueEntryUseCase>();


			// Dto assemblers
			services.AddScoped<ListingDetailDtoAssembler>();
			services.AddScoped<ListingGroupSummaryDtoAssembler>();
			services.AddScoped<ListingQueueEntryDtoAssembler>();


			services.ConfigureHttpJsonOptions(options => options.SerializerOptions.ConfigureJson());

			// Http clients
			services.AddHttpClient<ListingsHttpClient>(config =>
			{
				config.BaseAddress = new Uri($"http://{configuration["LISTINGS_SERVICE_HOST"]}:{configuration["LISTINGS_SERVICE_PORT"]}");
			});
			services.AddHttpClient<CatalogHttpClient>(config =>
			{
				config.BaseAddress = new Uri($"http://{configuration["CATALOG_SERVICE_HOST"]}:{configuration["CATALOG_SERVICE_PORT"]}");
			});
			services.AddHttpClient<UsersHttpClient>(config =>
			{
				config.BaseAddress = new Uri($"http://{configuration["USERS_SERVICE_HOST"]}:{configuration["USERS_SERVICE_PORT"]}");
			});



			return services;
		}
	}
}
