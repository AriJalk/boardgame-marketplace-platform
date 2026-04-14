using BoardGameMarket.CatalogService.Infrastructure.Seeders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Infrastructure.InMemorySeeders
{
	public class SeedRunner
	{
		private IEnumerable<ISeeder> _seeders;
		private ISeedValidator _seedValidator;
		public SeedRunner(IEnumerable<ISeeder> seeders, ISeedValidator seedValidator)
		{
			_seeders = seeders;
			_seedValidator = seedValidator;
		}

		public async Task SeedAsync(string seedPath)
		{
			if (await _seedValidator.IsSeeded())
			{
				return;
			}
			foreach (ISeeder seeder in _seeders)
			{
				await seeder.SeedAsync(seedPath);
			}
		}
	}
}
