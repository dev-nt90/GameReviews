namespace GameRatings.Api
{
	public static class ApiEndpoints
	{
		private const String ApiBase = "api";

		public static class Games
		{
			private const String Base = $"{ApiBase}/games";
			public const String Create = Base;
			public const String Get = $"{Base}/{{idOrSlug}}";
			public const String GetAll = Base;
			public const String Update = $"{Base}/{{id:guid}}";
			public const String Delete = $"{Base}/{{id:guid}}";
		}
	}
}
