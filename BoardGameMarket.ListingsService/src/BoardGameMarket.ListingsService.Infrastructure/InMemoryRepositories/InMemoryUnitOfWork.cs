using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.Repositories.DbContext;
using Microsoft.Extensions.Configuration;
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
        public Task<int> ExecuteAsync(string sql, object? param)
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            await action();
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            return await action();
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param)
        {
            throw new NotImplementedException();
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param)
        {
            throw new NotImplementedException();
        }
    }
}
