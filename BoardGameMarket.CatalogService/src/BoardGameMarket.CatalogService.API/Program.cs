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
bool isLocalBggClient = builder.Configuration.GetValue<bool>("LOCAL_BGG_CLIENT");

if (inMemory)
{
	builder.Services.AddInMemoryInfrastructure(builder.Configuration);
}
else
{
	builder.Services.AddInfrastructure(builder.Configuration);
}
if (isSeeding)
{
    builder.Services.AddSeeding();
}

if (isLocalBggClient)
{
	builder.Services.ConfigureLocalBggClient();
}
else
{
	builder.Services.ConfigureBggClient(builder.Configuration);
}

var app = builder.Build();

if (isSeeding)
{
	using (var scope = app.Services.CreateScope())
	{
		SeedRunner seeder = scope.ServiceProvider.GetRequiredService<SeedRunner>();
		string seedPath = builder.Configuration["SEED_ROOT"] ?? throw new InvalidOperationException("Seed root not configured");
		await seeder.SeedAsync(seedPath);
	}
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
