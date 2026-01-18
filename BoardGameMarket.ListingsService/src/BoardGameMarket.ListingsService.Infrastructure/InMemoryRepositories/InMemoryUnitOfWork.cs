using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories
{
	internal class InMemoryUnitOfWork : IUnitOfWork
	{
		public Task BeginTransactionAsync()
		{
			return Task.CompletedTask;
		}

		public Task CommitTransactionAsync()
		{
			return Task.CompletedTask;
		}

		public Task RollBackTransactionAsync()
		{
			return Task.CompletedTask;
		}

		//public Task<int> SaveChangesAsync()
		//{
		//	return Task.FromResult(1);
		//}
	}
}
