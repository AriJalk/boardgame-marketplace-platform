using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories.Seeders
{
	public interface ISeeder
	{
		Task SeedAsync(string seedPath);
	}
}
