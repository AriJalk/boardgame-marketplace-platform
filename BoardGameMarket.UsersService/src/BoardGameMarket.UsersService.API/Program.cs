using BoardGameMarket.UsersService.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices();
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();
