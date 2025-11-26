var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(policy =>
{
	policy.WithOrigins("https://localhost:4200");
});
app.MapControllers();

app.Run();
