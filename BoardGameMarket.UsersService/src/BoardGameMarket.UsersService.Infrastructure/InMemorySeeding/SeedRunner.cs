using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.UsersService.Infrastructure.InMemorySeeding
{
	public class SeedRunner
	{
		private readonly IEnumerable<ISeeder> _seeders;

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
