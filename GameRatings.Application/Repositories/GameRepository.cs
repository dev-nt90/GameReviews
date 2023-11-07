using Dapper;
using GameRatings.Application.Database;
using GameRatings.Application.Models;

namespace GameRatings.Application.Repositories
{
	public class GameRepository : IGameRepository
	{
		private readonly IDbConnectionFactory dbConnectionFactory;

		public GameRepository(IDbConnectionFactory dbConnectionFactory)
		{
			this.dbConnectionFactory = dbConnectionFactory;
		}

		public async Task<bool> CreateAsync(Game game)
		{
			using(var connection = await this.dbConnectionFactory.CreateConnectionAsync())
			using(var transaction = connection.BeginTransaction())
			{
				var result = await connection.ExecuteAsync(new CommandDefinition("""
					insert into games (id, slug, title, year_of_release)
					values(@Id, @Slug, @Title, @YearOfRelease)
					""", game));
				
				if(result > 0)
				{
					foreach(var genre in game.Genres)
					{
						await connection.ExecuteAsync(new CommandDefinition("""
							insert into genres (game_id, name)
							values(@GameId, @Name)
							""", new { GameId = game.Id, Name = genre }));
					}
				}

				transaction.Commit();

				return result > 0;
			}
		}

		public async Task<bool> DeleteByIdAsync(Guid id)
		{
			using(var connection = await this.dbConnectionFactory.CreateConnectionAsync())
			using(var transaction = connection.BeginTransaction())
			{
				await connection.ExecuteAsync(new CommandDefinition("""
					delete from genres where game_id=@id
					""", new { id }));

				var result = await connection.ExecuteAsync(new CommandDefinition("""
					delete from games where id=@id
					""", new { id }));

				transaction.Commit();

				return result > 0;
			}
		}

		public async Task<bool> ExistsByIdAsync(Guid id)
		{
			using(var connection = await this.dbConnectionFactory.CreateConnectionAsync())
			{
				return await connection.ExecuteScalarAsync<Boolean>(new CommandDefinition("""
					select count(1) from games where id = @id
					""", new { id }));
			}
		}

		public async Task<IEnumerable<Game>> GetAllAsync()
		{
			using(var connection = await this.dbConnectionFactory.CreateConnectionAsync())
			{
				// HACK: something (propbably psql) is causing these columns to come out of this query as all lowercase (e.g. yearofrelease)
				// that is, unless you explicitly specify the casing with the quotation syntax
				var result = await connection.QueryAsync(new CommandDefinition("""
					select 
						ga.id AS "Id",
						ga.title AS "Title",
						ga.year_of_release AS "YearOfRelease",
						string_agg(ge.name, ',') as "Genres"
					from games ga
					left join genres ge
						on ga.id = ge.game_id
					group by ga.id
					"""));

				return result.Select(g => new Game
				{
					Id = g.Id,
					Title = g.Title,
					YearOfRelease = g.YearOfRelease,
					Genres = Enumerable.ToList(g.Genres.Split(','))
				});
			}
		}

		public async Task<Game?> GetByIdAsync(Guid id)
		{
			using(var connection = await this.dbConnectionFactory.CreateConnectionAsync())
			{
				var game = await connection.QuerySingleOrDefaultAsync<Game>(
					new CommandDefinition("""
						select 
							ga.id AS "Id", 
							ga.slug as "Slug",
							ga.title AS "Title",
							ga.year_of_release AS "YearOfRelease" 
						from games ga
						where id=@id
						""", new { id }));

				if(game is null)
				{
					return null;
				}

				var genres = await connection.QueryAsync<String>(
					new CommandDefinition("""
						select name from genres where game_id = @id
						""", new { id }));

				foreach(var genre in genres)
				{
					game.Genres.Add(genre);
				}

				return game;
			}
		}

		public async Task<Game?> GetBySlugAsync(String slug)
		{
			using (var connection = await this.dbConnectionFactory.CreateConnectionAsync())
			{
				var game = await connection.QuerySingleOrDefaultAsync<Game>(
					new CommandDefinition("""
						select 
							ga.id AS "Id", 
							ga.slug as "Slug",
							ga.title AS "Title",
							ga.year_of_release AS "YearOfRelease" 
						from games ga 
						where slug=@slug
						""", new { slug }));

				if (game is null)
				{
					return null;
				}

				var genres = await connection.QueryAsync<String>(
					new CommandDefinition("""
						select name from genres where game_id = @id
						""", new { game.Id }));

				foreach (var genre in genres)
				{
					game.Genres.Add(genre);
				}

				return game;
			}
		}

		public async Task<bool> UpdateAsync(Game game)
		{
			using(var connection = await this.dbConnectionFactory.CreateConnectionAsync())
			using (var transaction = connection.BeginTransaction())
			{
				await connection.ExecuteAsync(new CommandDefinition("""
					delete from genres where game_id = @id
					""", new { id = game.Id }));

				foreach(var genre in game.Genres)
				{
					await connection.ExecuteAsync(new CommandDefinition("""
						insert into genres(game_id, name)
						values(@Id, @Name)
						""", new { Id = game.Id, Name = genre }));
				}

				var result = await connection.ExecuteAsync(new CommandDefinition("""
					update games
					set 
						slug = @Slug,
						title = @Title,
						year_of_release = @YearOfRelease
					where id = @id
					""", game));

				transaction.Commit();
				return result > 0;
			}
		}
	}
}
