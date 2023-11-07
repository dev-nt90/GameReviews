using GameRatings.Application.Models;

namespace GameRatings.Application.Repositories
{
	public interface IGameRepository
	{
		Task<Boolean> CreateAsync(Game game);
		Task<Game?> GetByIdAsync(Guid id);
		Task<Game?> GetBySlugAsync(String slug);
		Task<IEnumerable<Game>> GetAllAsync();
		Task<Boolean> UpdateAsync(Game game);
		Task<Boolean> DeleteByIdAsync(Guid id);
		Task<Boolean> ExistsByIdAsync(Guid id);
	}
}
