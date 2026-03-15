using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.UsersService.Infrastructure.Repositories
{
	internal class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _db;
		private readonly ILogger<UnitOfWork> _logger;


		public UnitOfWork(ApplicationDbContext db, ILogger<UnitOfWork> logger)
		{
			_db = db;
			_logger = logger;
		}


        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            IExecutionStrategy strategy = _db.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _db.Database.BeginTransactionAsync();

                try
                {
                    await action();

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            IExecutionStrategy strategy = _db.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () => 
            {
                await using var transaction = await _db.Database.BeginTransactionAsync();

                try
                {
                    var result = await action();

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
		
	}

}
