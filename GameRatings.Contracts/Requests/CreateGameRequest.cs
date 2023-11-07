
namespace GameRatings.Contracts.Requests
{
	public class CreateGameRequest
	{
		public required String Title { get; init; }
		public required Int32 YearOfRelease { get; init; }
		public required IEnumerable<String> Genres { get; init; } = Enumerable.Empty<String>();
	}
}
