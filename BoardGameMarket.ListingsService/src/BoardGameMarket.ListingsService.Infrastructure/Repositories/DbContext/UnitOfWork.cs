using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories.DbContext
{
    internal class UnitOfWork : IUnitOfWork
	{
		public IDbConnection Connection { get; private set; }
        public IDbTransaction? Transaction { get; private set; }

		public UnitOfWork(PostgresDbContext dbContext)
		{
            Connection = dbContext.Connection;
		}

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            using var transaction = Connection.BeginTransaction();
            Transaction = transaction;
            try
            {
                await action();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            finally
            {
                Transaction = null;
            }
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            using var transaction = Connection.BeginTransaction();
            Transaction = transaction;
            try
            {
                var result = await action();
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                Transaction = null;
            }
        }
    }
}
