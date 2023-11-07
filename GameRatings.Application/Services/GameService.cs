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

		public async Task<bool> CreateAsync(Game game)
		{
			await this.gameValidator.ValidateAndThrowAsync(game);
			return await this.gameRepository.CreateAsync(game);
		}

		public Task<bool> DeleteByIdAsync(Guid id)
		{
			return this.gameRepository.DeleteByIdAsync(id);
		}

		public Task<bool> ExistsByIdAsync(Guid id)
		{
			return this.gameRepository.ExistsByIdAsync(id);
		}

		public Task<IEnumerable<Game>> GetAllAsync()
		{
			return this.gameRepository.GetAllAsync();
		}

		public Task<Game?> GetByIdAsync(Guid id)
		{
			return this.gameRepository.GetByIdAsync(id);
		}

		public Task<Game?> GetBySlugAsync(string slug)
		{
			return this.gameRepository.GetBySlugAsync(slug);
		}

		public async Task<Game?> UpdateAsync(Game game)
		{
			await this.gameValidator.ValidateAndThrowAsync(game);
			var gameExists = await this.gameRepository.ExistsByIdAsync(game.Id);

			if(!gameExists)
			{
				return null;
			}

			await this.gameRepository.UpdateAsync(game);
			return game;
		}
	}
}
