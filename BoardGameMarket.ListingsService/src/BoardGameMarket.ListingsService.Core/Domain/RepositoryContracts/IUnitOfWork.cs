using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(Func<Task> action);
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action);
        Task<int> ExecuteAsync(string sql, object? param);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param);
        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param);

	}
}
