using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore.Storage;
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

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
			await action();
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            return await action();
        }

        public async Task<int> SaveChangesAsync()
		{
			await _db.SaveChangesAsync();
			return 1;
		}
	}
}
