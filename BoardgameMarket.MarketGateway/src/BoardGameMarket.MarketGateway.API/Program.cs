using System.Text.Json;
using BoardGameMarket.MarketGateway.API.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        string[]? allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        if (allowedOrigins == null)
        {
            throw new ArgumentNullException("Cors origins can't be null");
        }
        policy.WithOrigins(allowedOrigins)
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
