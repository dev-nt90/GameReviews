using FluentValidation;
using GameRatings.Application.Models;
using GameRatings.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRatings.Application.Services
{
	public class GameService : IGameService
	{
		private readonly IGameRepository gameRepository;
		private readonly IValidator<Game> gameValidator;

		public GameService(IGameRepository gameRepository, IValidator<Game> gameValidator)
		{
			this.gameRepository = gameRepository;
			this.gameValidator = gameValidator;
		}

		public async Task<bool> CreateAsync(Game game, CancellationToken cancellationToken = default)
		{
			await this.gameValidator.ValidateAndThrowAsync(game);
			return await this.gameRepository.CreateAsync(game, cancellationToken);
		}

		public Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return this.gameRepository.DeleteByIdAsync(id, cancellationToken);
		}

		public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return this.gameRepository.ExistsByIdAsync(id, cancellationToken);
		}

		public Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return this.gameRepository.GetAllAsync(cancellationToken);
		}

		public Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return this.gameRepository.GetByIdAsync(id, cancellationToken);
		}

		public Task<Game?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
		{
			return this.gameRepository.GetBySlugAsync(slug, cancellationToken);
		}

		public async Task<Game?> UpdateAsync(Game game, CancellationToken cancellationToken)
		{
			await this.gameValidator.ValidateAndThrowAsync(game);
			var gameExists = await this.gameRepository.ExistsByIdAsync(game.Id, cancellationToken);

			if(!gameExists)
			{
				return null;
			}

			await this.gameRepository.UpdateAsync(game, cancellationToken);
			return game;
		}
	}
}
