using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Infrastructure.InMemorySeeders
{
	public class SeedRunner
	{
		private IEnumerable<ISeeder> _seeders;
		public SeedRunner(IEnumerable<ISeeder> seeders)
		{
			_seeders = seeders;
		}

		public async Task SeedAsync(string seedPath)
		{
			foreach (ISeeder seeder in _seeders)
			{
				await seeder.SeedAsync(seedPath);
			}
		}
	}
}
