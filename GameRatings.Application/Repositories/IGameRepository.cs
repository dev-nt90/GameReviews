using GameRatings.Application.Models;

namespace GameRatings.Application.Repositories
{
	public interface IGameRepository
	{
		Task<Boolean> CreateAsync(Game game, CancellationToken cancellationToken = default);
		Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<Game?> GetBySlugAsync(String slug, CancellationToken cancellationToken = default);
		Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<Boolean> UpdateAsync(Game game, CancellationToken cancellationToken = default);
		Task<Boolean> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<Boolean> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
	}
}
