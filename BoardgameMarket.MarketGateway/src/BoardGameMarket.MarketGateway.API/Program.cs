using System.Text.Json;
using BoardGameMarket.MarketGateway.API.Configuration;
using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Services;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Services;
using BoardGameMarket.MarketGateway.API.Modules.Queues.Services;
using BoardGameMarket.MarketGateway.API.Modules.Users.Services;
using BoardGameMarket.MarketGateway.API.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(policy =>
{
	policy.WithOrigins("https://localhost:4200").AllowAnyMethod().AllowAnyHeader();
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
