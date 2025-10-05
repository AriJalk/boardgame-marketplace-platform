using BoardGameMarket.UsersService.Core;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using BoardGameMarket.UsersService.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices();

#if (DEBUG)
builder.Services.AddInMemoryInfrastructure();
#else
builder.Services.AddPostgresInfrastructure();
#endif

builder.Services.AddControllers()
	//.AddJsonOptions(options =>
	//{
	//	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<RoleType>());
	//})
	;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
