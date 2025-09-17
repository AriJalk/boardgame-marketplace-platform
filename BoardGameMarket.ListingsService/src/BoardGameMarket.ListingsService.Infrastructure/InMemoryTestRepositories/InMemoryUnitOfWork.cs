using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories
{
	internal class InMemoryUnitOfWork : IUnitOfWork
	{
		public Task BeginTransactionAsync()
		{
			throw new NotImplementedException();
		}

		public Task CommitTransactionAsync()
		{
			throw new NotImplementedException();
		}

		public Task RollBackTransactionAsync()
		{
			throw new NotImplementedException();
		}

		public Task<int> SaveChangesAsync()
		{
			throw new NotImplementedException();
		}
	}
}
