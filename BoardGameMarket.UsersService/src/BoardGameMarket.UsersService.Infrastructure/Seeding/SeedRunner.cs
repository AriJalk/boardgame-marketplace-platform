using BoardGameMarket.UsersService.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.UsersService.Infrastructure.Seeding
{
	public class SeedRunner
	{
		private readonly IEnumerable<ISeeder> _seeders;
		private readonly ApplicationDbContext _dbContext;

		public SeedRunner(IEnumerable<ISeeder> seeders, ApplicationDbContext dbContext)
		{
			_seeders = seeders;
			_dbContext = dbContext;
		}

		public async Task SeedAsync(string seedPath)
		{
			if (_dbContext.SeedMarkers.Any(m => m.Name == "catalog"))
			{
				return;
			}
			foreach (ISeeder seeder in _seeders)
			{
				await seeder.SeedAsync(seedPath);
			}
			SeedMarker seedMarker = new SeedMarker() { Name = "catalog", SeededAt = DateTime.UtcNow };
			await _dbContext.SeedMarkers.AddAsync(seedMarker);
			await _dbContext.SaveChangesAsync();
		}
	}
}
