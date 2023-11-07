using FluentValidation;
using GameRatings.Application.Models;
using GameRatings.Application.Repositories;

namespace GameRatings.Application.Validators
{
	public class GameValidator : AbstractValidator<Game>
	{
		private readonly IGameRepository gameRepository;

		public GameValidator(IGameRepository gameRepository)
		{
			this.gameRepository = gameRepository;
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Genres).NotEmpty();
			RuleFor(x => x.Title).NotEmpty();
			RuleFor(x => x.YearOfRelease).LessThanOrEqualTo(DateTime.UtcNow.Year);
			RuleFor(x => x.Slug).MustAsync(ValidateSlug).WithMessage("This game already exists in the system");
		}

		private async Task<Boolean> ValidateSlug(Game game, String slug, CancellationToken cancellationToken)
		{
			var existingGame = await this.gameRepository.GetBySlugAsync(slug);

			return
				// return true if the entity already exists and matches the ID on record
				(existingGame != null && existingGame.Id == game.Id) || 

				// return true if this is a new entity
				existingGame is null;
		}
	}
}
