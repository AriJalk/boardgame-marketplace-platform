using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.UsersService.Infrastructure.InMemorySeeding
{
	public interface ISeeder
	{
		Task SeedAsync(string seedPath);
	}
}
