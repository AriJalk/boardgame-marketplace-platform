using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Infrastructure.InMemorySeeders
{
	public interface ISeeder
	{
		Task SeedAsync(string seedPath);
	}
}
