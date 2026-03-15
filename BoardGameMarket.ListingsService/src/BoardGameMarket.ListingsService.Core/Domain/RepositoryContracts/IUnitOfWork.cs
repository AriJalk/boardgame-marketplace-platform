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
        IDbConnection Connection { get; }
        IDbTransaction? Transaction { get; }
        //Task<int> SaveChangesAsync();
        Task ExecuteInTransactionAsync(Func<Task> action);
		Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action);
	}
}
