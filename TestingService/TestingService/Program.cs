using TestingService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<ListingsService>();

var app = builder.Build();

app.UseCors(policy =>
{
	policy.WithOrigins("https://localhost:4200").AllowAnyMethod().AllowAnyHeader();
});
app.MapControllers();

app.Run();
