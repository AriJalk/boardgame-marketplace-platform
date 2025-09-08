using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace BoardGameMarket.UsersService.Infrastructure.Repositories
{
	internal class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _db;
		private IDbContextTransaction? _transaction;

		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
		}

		public Task<int> SaveChangesAsync()
		{
			if (_transaction != null)
			{
				throw new InvalidOperationException("Can't save changes explicitly while transaction is active, use CommitTransactionAsync instead");
			}
			return _db.SaveChangesAsync();
		}

		public async Task BeginTransactionAsync()
		{
			_transaction = await _db.Database.BeginTransactionAsync();
		}

		public async Task CommitTransactionAsync()
		{
			if (_transaction == null) throw new InvalidOperationException("No transaction started");

			try
			{
				await _transaction.CommitAsync();
			}
			catch
			{
				await _transaction.RollbackAsync();
				throw;
			}
			finally
			{
				await _transaction.DisposeAsync();
				_transaction = null;
			}
		}


		public async Task RollBackTransactionAsync()
		{
			if (_transaction == null)
			{
				return;
			}

			await _transaction.RollbackAsync();
			await _transaction.DisposeAsync();
			_transaction = null;
		}
	}

}
