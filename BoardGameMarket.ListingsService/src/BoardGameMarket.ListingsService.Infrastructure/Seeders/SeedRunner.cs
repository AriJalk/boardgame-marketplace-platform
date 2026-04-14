using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.Repositories.DbContext;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.Seeders
{
	public class SeedRunner
	{
		private readonly IEnumerable<ISeeder> _rootSeeders;
		private readonly QueueSeeder _queueSeeder;
		private readonly IUnitOfWork _unitOfWork;

		public SeedRunner(IEnumerable<ISeeder> rootSeeders, QueueSeeder queueSeeder, IUnitOfWork unitOfWork)
		{
			_rootSeeders = rootSeeders;
			_queueSeeder = queueSeeder;
			_unitOfWork = unitOfWork;
		}

		public async Task SeedAsync(string seedPath)
		{
            var exists = await _unitOfWork.QueryFirstOrDefaultAsync<bool>(
				"SELECT EXISTS (SELECT 1 FROM internal.seed_markers WHERE name = @name)",
				new { name = "catalog" });
			if (!exists)
			{
                await SeedRoot(seedPath);
                await _queueSeeder.SeedAsync(seedPath);
				try
				{
                    await _unitOfWork.ExecuteAsync(@"
						INSERT INTO internal.seed_markers (name, seeded_at) 
						VALUES (@name, NOW())",
						new { name = "catalog" });
                }
                catch (PostgresException ex) when (ex.SqlState == "23505")
                {
					return;
                }
            }
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
