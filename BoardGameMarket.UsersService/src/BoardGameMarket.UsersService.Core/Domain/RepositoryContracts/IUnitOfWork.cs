using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Domain.RepositoryContracts
{
	public interface IUnitOfWork
	{
		Task<int> SaveChangesAsync();
		Task BeginTransactionAsync();
		Task CommitTransactionAsync();
		Task RollBackTransactionAsync();
	}
}
