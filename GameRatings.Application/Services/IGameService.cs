using GameRatings.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRatings.Application.Services
{
	public interface IGameService
	{
		Task<Boolean> CreateAsync(Game game, CancellationToken cancellationToken);
		Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
		Task<Game?> GetBySlugAsync(String slug, CancellationToken cancellationToken);
		Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken);
		Task<Game?> UpdateAsync(Game game, CancellationToken cancellationToken);
		Task<Boolean> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
	}
}
