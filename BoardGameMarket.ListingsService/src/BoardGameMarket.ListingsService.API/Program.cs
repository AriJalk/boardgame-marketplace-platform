using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.API.Middleware;
using BoardGameMarket.ListingsService.Core;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure;
using BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore();

if (builder.Configuration.GetValue<bool>("IN_MEMORY"))
{
	builder.Services.AddInMemoryInfrastructure();
}
else
{
	builder.Services.AddInfrastructure();
}


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();

JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
jsonOptions.PropertyNameCaseInsensitive = true;
jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
builder.Services.AddSingleton(jsonOptions);

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseCurrentUserMiddleware();

if (builder.Configuration.GetValue<bool>("IN_MEMORY"))
{
	using (var scope = app.Services.CreateScope())
	{
		InMemoryDataSeeder seeder = new InMemoryDataSeeder(
			scope.ServiceProvider.GetRequiredService<IListingGroupRepository>(),
			scope.ServiceProvider.GetRequiredService<IListingRepository>(),
			scope.ServiceProvider.GetRequiredService<IListingItemRepository>(),
			scope.ServiceProvider.GetRequiredService<IListingQueueEntryRepository>(),
			scope.ServiceProvider.GetRequiredService<IListingSoldRepository>(),
			jsonOptions, Path.Combine(builder.Environment.ContentRootPath, "SeedData")
			);

		Debug.WriteLine(builder.Environment.ContentRootPath);
		await seeder.SeedAsync();
	}
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
