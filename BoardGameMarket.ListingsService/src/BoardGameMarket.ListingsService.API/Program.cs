using BoardGameMarket.ListingsService.API.Middleware;
using BoardGameMarket.ListingsService.Core;
using BoardGameMarket.ListingsService.Infrastructure;
using BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories.Seeders;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore();

bool inMemory = builder.Configuration.GetValue<bool>("IN_MEMORY");
bool isSeeding = builder.Configuration.GetValue<bool>("IS_SEEDING");
if (inMemory)
{
	builder.Services.AddInMemoryInfrastructure();
	if (isSeeding)
	{
		builder.Services.AddInMemorySeeding();
	}
}
else
{
	builder.Services.AddInfrastructure();
}


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();



var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseCurrentUserMiddleware();

if (inMemory && isSeeding)
{
	using (var scope = app.Services.CreateScope())
	{
		InMemorySeedRunner seedRunner = scope.ServiceProvider.GetRequiredService<InMemorySeedRunner>();
		string seedPath = builder.Configuration["SEED_ROOT"] ?? throw new InvalidOperationException("Seed root not configured");
		await seedRunner.SeedAsync(seedPath);
	}
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
