using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRatings.Contracts.Responses
{
	public class GameResponse
	{
		public required Guid Id { get; init; }
		public required String Slug { get; init; }
		public required String Title { get; init; }
		public required Int32 YearOfRelease { get; init; }
		public required IEnumerable<String> Genres { get; init; } = Enumerable.Empty<String>();
	}
}
