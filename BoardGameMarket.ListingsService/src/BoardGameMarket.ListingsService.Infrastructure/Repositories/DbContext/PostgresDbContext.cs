using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories.DbContext
{
    public class PostgresDbContext : IDisposable
	{
		private readonly IDbConnection _connection;
		public IDbConnection Connection => _connection;

		public PostgresDbContext(IConfiguration configuration)
		{
			string connectionStringTemplate = configuration.GetConnectionString("PostgresConnection")!;
			string connectionString = connectionStringTemplate
				.Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
				.Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"))
				.Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"))
				.Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER"))
				.Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"));
			_connection = new NpgsqlConnection(connectionString);
		}

        public void Dispose()
        {
			_connection.Close();
        }
    }
}
