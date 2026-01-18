using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories.Seeders
{
	public class InMemorySeedRunner
	{
		private readonly IEnumerable<ISeeder> _rootSeeders;
		private readonly QueueSeeder _queueSeeder; 

		public InMemorySeedRunner(IEnumerable<ISeeder> rootSeeders, QueueSeeder queueSeeder)
		{
			_rootSeeders = rootSeeders;
			_queueSeeder = queueSeeder;
		}

		public async Task SeedAsync(string seedPath)
		{
			await SeedRoot(seedPath);
			await _queueSeeder.SeedAsync(seedPath);
		}

		private async Task SeedRoot(string seedPath)
		{
			foreach (ISeeder seeder in _rootSeeders)
			{
				await seeder.SeedAsync(seedPath);
			}
		}
	}
}
