using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.UsersService.Infrastructure.Repositories
{
	internal class InMemoryUnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _db;

		public InMemoryUnitOfWork(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task BeginTransactionAsync()
		{
			
		}

		public async Task CommitTransactionAsync()
		{
			await _db.SaveChangesAsync();
		}

		public async Task RollBackTransactionAsync()
		{
			
		}

		public async Task<int> SaveChangesAsync()
		{
			await _db.SaveChangesAsync();
			return 1;
		}
	}
}
