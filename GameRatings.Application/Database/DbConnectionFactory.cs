using Npgsql;
using System.Data;

namespace GameRatings.Application.Database
{
	public interface IDbConnectionFactory
	{
		Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
	}

	public class NpgsqlConnectionFactory : IDbConnectionFactory
	{
		private readonly String connectionString;

		public NpgsqlConnectionFactory(String connectionString)
		{
			this.connectionString = connectionString;
		}

		public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
		{
			var connection = new NpgsqlConnection(this.connectionString);
			await connection.OpenAsync(cancellationToken);
			return connection;
		}
	}
}
