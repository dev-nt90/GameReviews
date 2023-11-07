using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameRatings.Application.Models
{
	public partial class Game
	{
		public required Guid Id { get; init; }
		public string Slug => GenerateSlug();
		public required string Title { get; set; }
		public required int YearOfRelease { get; set; }
		public required List<string> Genres { get; init; } = new();

		private string GenerateSlug()
		{
			var sluggedTitle = SlugRegex().Replace(Title, string.Empty).ToLower().Replace(" ", "-");
			return $"{sluggedTitle}-{YearOfRelease}";
		}

		[GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 10)]
		private static partial Regex SlugRegex();
	}
}
