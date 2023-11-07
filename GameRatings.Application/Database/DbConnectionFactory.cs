using Npgsql;
using System.Data;

namespace GameRatings.Application.Database
{
	public interface IDbConnectionFactory
	{
		Task<IDbConnection> CreateConnectionAsync();
	}

	public class NpgsqlConnectionFactory : IDbConnectionFactory
	{
		private readonly String connectionString;

		public NpgsqlConnectionFactory(String connectionString)
		{
			this.connectionString = connectionString;
		}

		public async Task<IDbConnection> CreateConnectionAsync()
		{
			var connection = new NpgsqlConnection(this.connectionString);
			await connection.OpenAsync();
			return connection;
		}
	}
}
