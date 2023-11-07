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
		Task<Boolean> CreateAsync(Game game);
		Task<Game?> GetByIdAsync(Guid id);
		Task<Game?> GetBySlugAsync(String slug);
		Task<IEnumerable<Game>> GetAllAsync();
		Task<Game?> UpdateAsync(Game game);
		Task<Boolean> DeleteByIdAsync(Guid id);
	}
}
