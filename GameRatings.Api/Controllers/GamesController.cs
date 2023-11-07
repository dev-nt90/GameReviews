using GameRatings.Api.Mapping;
using GameRatings.Application.Models;
using GameRatings.Application.Repositories;
using GameRatings.Application.Services;
using GameRatings.Contracts.Requests;
using GameRatings.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GameRatings.Api.Controllers
{
	[ApiController]
	public class GamesController : ControllerBase
	{
		private readonly IGameService gameService;
		public GamesController(IGameService gameRepository)
		{
			this.gameService = gameRepository;
		}

		[HttpPost(ApiEndpoints.Games.Create)]
		public async Task<IActionResult> Create(
			[FromBody]CreateGameRequest request, 
			CancellationToken cancellationToken)
		{
			var game = request.MapToGame();

			var result = await this.gameService.CreateAsync(game, cancellationToken);

			var gameResponse = game.MapToResponse();

			//return Created($"/{ApiEndpoints.Games.Create}/{gameResponse.Id}", gameResponse);
			return CreatedAtAction(nameof(Create), new {id = gameResponse.Id, gameResponse});
		}

		[HttpGet(ApiEndpoints.Games.Get)]
		public async Task<IActionResult> Get(
			[FromRoute] String idOrSlug,
			CancellationToken cancellationToken)
		{
			var game = Guid.TryParse(idOrSlug, out var gameId)
				? await this.gameService.GetByIdAsync(gameId, cancellationToken)
				: await this.gameService.GetBySlugAsync(idOrSlug, cancellationToken);

			if(game is null)
			{
				return NotFound();
			}

			return Ok(game.MapToResponse());
		}

		[HttpGet(ApiEndpoints.Games.GetAll)]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var games = await this.gameService.GetAllAsync(cancellationToken);
			var gamesResponse = games.MapToResponse();

			return Ok(gamesResponse);
		}

		[HttpPut(ApiEndpoints.Games.Update)]
		public async Task<IActionResult> Update(
			[FromRoute] Guid id,
			[FromBody] UpdateGameRequest request, 
			CancellationToken cancellationToken)
		{
			var game = request.MapToGame(id);
			var updatedGame = await this.gameService.UpdateAsync(game, cancellationToken);

			if (updatedGame is null)
			{
				return NotFound();
			}

			var response = game.MapToResponse();
			return Ok(response);
		}

		[HttpDelete(ApiEndpoints.Games.Delete)]
		public async Task<IActionResult> Delete(
			[FromRoute] Guid id, 
			CancellationToken cancellationToken)
		{
			var deleted = await this.gameService.DeleteByIdAsync(id, cancellationToken);
			if (!deleted)
			{
				return NotFound();
			}

			return Ok();
		}
	}
}
