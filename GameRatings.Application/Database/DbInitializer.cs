using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRatings.Application.Database
{
	public class DbInitializer
	{
		private readonly IDbConnectionFactory connectionFactory;

		public DbInitializer(IDbConnectionFactory connectionFactory)
		{
			this.connectionFactory = connectionFactory;
		}

		public async Task InitializeAsync()
		{
			using(var connection = await this.connectionFactory.CreateConnectionAsync())
			{
				await connection.ExecuteAsync("""
					create table if not exists games(
						id UUID primary key,
						slug TEXT not null,
						title TEXT not null,
						year_of_release integer not null
					)
					""");

				await connection.ExecuteAsync("""
					create unique index concurrently if not exists games_slug_idx
					on games
					using btree(slug);
					""");

				await connection.ExecuteAsync("""
					create table if not exists genres (
						game_id UUID references games(id),
						name TEXT not null
					)
					""");
			}
		}
	}
}
