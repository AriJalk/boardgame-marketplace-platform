
using System.Text.Json;
using System.Text.Json.Serialization;
using BoardGameMarket.MarketGateway.API.Configuration;
using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Services;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Services;
using BoardGameMarket.MarketGateway.API.Modules.Queues.Services;
using BoardGameMarket.MarketGateway.API.Modules.Users.Services;

var builder = WebApplication.CreateBuilder(args);

JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
jsonOptions.ConfigureJson();

builder.Services.AddControllers().
	AddJsonOptions(options => options.JsonSerializerOptions.ConfigureJson());

builder.Services.AddSingleton(jsonOptions);
builder.Services.AddSingleton<CatalogService>();
builder.Services.AddSingleton<ListingsDataService>();
builder.Services.AddSingleton<QueuesDataService>();
builder.Services.AddSingleton<UsersService>();

builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.ConfigureJson());

var app = builder.Build();

app.UseCors(policy =>
{
	policy.WithOrigins("https://localhost:4200").AllowAnyMethod().AllowAnyHeader();
});
app.MapControllers();

app.Run();
