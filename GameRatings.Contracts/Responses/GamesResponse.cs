using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRatings.Contracts.Responses
{
	public class GamesResponse
	{
		public required IEnumerable<GameResponse> Items { get; init; } = Enumerable.Empty<GameResponse>();
	}
}
