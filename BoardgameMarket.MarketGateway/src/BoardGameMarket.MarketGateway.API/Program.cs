using BoardGameMarket.MarketGateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();


//app.UseHsts();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
