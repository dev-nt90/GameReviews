using FluentValidation;
using GameRatings.Application.Database;
using GameRatings.Application.Repositories;
using GameRatings.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameRatings.Application
{
	public static class ApplicationServiceExtensionCollections
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddSingleton<IGameRepository, GameRepository>();
			services.AddSingleton<IGameService, GameService>();
			services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
			return services;
		}

		public static IServiceCollection AddDatabase(this IServiceCollection services, String connectionString)
		{
			services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
			services.AddSingleton<DbInitializer>();
			return services;
		}
	}
}
