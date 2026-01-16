using BoardGameMarket.CatalogService.Core;
using BoardGameMarket.CatalogService.Infrastructure;
using BoardGameMarket.CatalogService.Infrastructure.InMemorySeeders;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
jsonOptions.PropertyNameCaseInsensitive = true;
jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
builder.Services.AddSingleton(jsonOptions);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

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
	builder.Services.AddInfrastructure(builder.Configuration);
}

var app = builder.Build();

if (inMemory && isSeeding)
{
	using (var scope = app.Services.CreateScope())
	{
		InMemorySeedRunner seeder = scope.ServiceProvider.GetRequiredService<InMemorySeedRunner>();
		string seedPath = Path.Combine(builder.Environment.ContentRootPath, "SeedData");
		await seeder.SeedAsync(seedPath);
	}
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
