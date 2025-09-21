using BoardGameMarket.ListingsService.Core;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure;
using BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore();
builder.Services.AddInMemoryInfrastructure();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
	InMemoryDataSeeder seeder = new InMemoryDataSeeder(
		scope.ServiceProvider.GetRequiredService<IListingGroupRepository>(),
		scope.ServiceProvider.GetRequiredService<IListingRepository>(),
		scope.ServiceProvider.GetRequiredService<IListingItemRepository>(),
		scope.ServiceProvider.GetRequiredService<IListingQueueEntryRepository>(),
		scope.ServiceProvider.GetRequiredService<IListingSoldRepository>()
		);

	await seeder.SeedAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
