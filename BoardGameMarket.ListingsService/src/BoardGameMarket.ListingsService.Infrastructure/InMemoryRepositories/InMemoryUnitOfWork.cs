using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories
{
    internal class InMemoryUnitOfWork : IUnitOfWork
    {
        public IDbConnection Connection => throw new NotImplementedException();

        public IDbTransaction? Transaction => throw new NotImplementedException();

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            await action();
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            return await action();
        }
    }
}
