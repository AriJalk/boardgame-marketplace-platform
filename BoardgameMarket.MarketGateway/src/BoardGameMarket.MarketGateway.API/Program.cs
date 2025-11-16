using BoardGameMarket.MarketGateway.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.Authority = $"https://{builder.Configuration["TENANT_NAME"]}.ciamlogin.com/{builder.Configuration["TENANT_ID"]}/v2.0";
		options.Audience = builder.Configuration["TENANT_CLIENT_ID"];
	});

var app = builder.Build();


//app.UseHsts();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();