using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories.DbContext
{
	internal class UnitOfWork : IUnitOfWork
	{
		private readonly IDbConnection _connection;
		private IDbTransaction? _transaction;

		public UnitOfWork(PostgresDbContext dbContext)
		{
			_connection = dbContext.Connection;
		}

		public Task BeginTransactionAsync()
		{
			if (_transaction != null)
			{
				throw new InvalidOperationException("Transaction already running");
			}
			_transaction = _connection.BeginTransaction();
			return Task.CompletedTask;
		}

		public Task CommitTransactionAsync()
		{
			if (_transaction == null)
			{
				throw new InvalidOperationException("Transaction not running");
			}
			try
			{
				_transaction.Commit();
			}
			catch
			{
				_transaction.Rollback();
				throw;
			}
			finally
			{
				_transaction.Dispose();
				_transaction = null;
			}
			return Task.CompletedTask;
		}

        public void Dispose()
        {
			_transaction?.Dispose();
        }

        public Task RollBackTransactionAsync()
		{
			if (_transaction == null)
			{
				throw new InvalidOperationException("Transaction not running");
			}

			_transaction.Rollback();
			return Task.CompletedTask;
		}

		//public Task<int> SaveChangesAsync()
		//{
		//	throw new NotImplementedException();
		//}
	}
}
