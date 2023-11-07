using GameRatings.Application.Models;
using GameRatings.Contracts.Requests;
using GameRatings.Contracts.Responses;
using Microsoft.AspNetCore.Http.Features;

namespace GameRatings.Api.Mapping
{
	public static class ContractMapping
	{
		public static Game MapToGame(this CreateGameRequest request)
		{
			return new Game
			{
				Id = Guid.NewGuid(),
				Title = request.Title,
				YearOfRelease = request.YearOfRelease,
				Genres = request.Genres.ToList()
			};
		}

		public static Game MapToGame(this UpdateGameRequest request, Guid id)
		{
			return new Game
			{
				Id = id,
				Title = request.Title,
				YearOfRelease = request.YearOfRelease,
				Genres = request.Genres.ToList()
			};
		}

		public static GameResponse MapToResponse(this Game game)
		{
			return new GameResponse
			{
				Id = game.Id,
				Title = game.Title,
				Slug = game.Slug,
				YearOfRelease = game.YearOfRelease,
				Genres = game.Genres
			};
		}

		public static GamesResponse MapToResponse(this IEnumerable<Game> games)
		{
			return new GamesResponse
			{
				Items = games.Select(MapToResponse)
			};
		}
	}
}
