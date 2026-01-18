using BoardGameMarket.UsersService.Core;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using BoardGameMarket.UsersService.Infrastructure;
using BoardGameMarket.UsersService.Infrastructure.InMemorySeeding;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices();

bool inMemory = builder.Configuration.GetValue<bool>("IN_MEMORY");
bool isSeeding = builder.Configuration.GetValue<bool>("IS_SEEDING");
if (inMemory)
{
	builder.Services.AddInMemoryInfrastructure();
	if (isSeeding)
	{
		JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
		jsonOptions.PropertyNameCaseInsensitive = true;
		jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
		builder.Services.AddSingleton(jsonOptions);

		builder.Services.AddInMemorySeeding();
	}
}
else
{
	builder.Services.AddPostgresInfrastructure(builder.Configuration);
}


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (inMemory && isSeeding)
{
	using (var scope = app.Services.CreateScope())
	{
		SeedRunner seedRunner = scope.ServiceProvider.GetRequiredService<SeedRunner>();
		string seedPath = builder.Configuration["SEED_ROOT"] ?? throw new InvalidOperationException("Seed root not configured");
		await seedRunner.SeedAsync(seedPath);
	};
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
