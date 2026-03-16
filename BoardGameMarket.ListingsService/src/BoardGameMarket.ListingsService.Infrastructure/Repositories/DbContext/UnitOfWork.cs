using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Dapper;
using Npgsql;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories.DbContext
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private readonly AsyncRetryPolicy _retryPolicy;
        private IDbTransaction? _transaction;

        public UnitOfWork(PostgresDbContext dbContext, AsyncRetryPolicy retryPolicy)
        {
            _connection = dbContext.Connection;
            _retryPolicy = retryPolicy;
        }

        private async Task<T> WithConnectionAsync<T>(Func<IDbConnection, IDbTransaction?, Task<T>> action)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                if (_transaction != null)
                {
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    return await action(_connection, _transaction);
                }

                await using var conn = new NpgsqlConnection(_connection.ConnectionString);
                await conn.OpenAsync();

                return await action(conn, null);
            });
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            if (_transaction != null)
            {
                await action();
                return;
            }
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            using var transaction = _connection.BeginTransaction();
            _transaction = transaction;
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
                _transaction = null;
                _connection.Close();
            }
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            if (_transaction != null)
            {
                return await action();
            }
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            using var transaction = _connection.BeginTransaction();
            _transaction = transaction;
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
                _transaction = null;
                _connection.Close();
            }
        }


        public async Task<int> ExecuteAsync(string sql, object? param)
        {
            return await WithConnectionAsync((conn, tx) => conn.ExecuteAsync(sql, param, tx));
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param)
        {
            return await WithConnectionAsync((conn, tx) => conn.QueryAsync<T>(sql, param, tx));
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param)
        {
            return await WithConnectionAsync((conn, tx) => conn.QueryFirstOrDefaultAsync<T>(sql, param, tx));
        }
    }
}
