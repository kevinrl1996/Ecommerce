using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Ordering.API.Extensions
{
	public static class DbExtension
	{
		public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
			where TContext : DbContext
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var logger = services.GetRequiredService<ILogger<TContext>>();
				var context = services.GetService<TContext>();

				try
				{
					logger.LogInformation($"Migración de base de datos iniciada: {typeof(TContext).Name}");
					//retry strategy
					var retry = Policy.Handle<SqlException>()
						.WaitAndRetry(
							retryCount: 5,
							sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
							onRetry: (exception, span, count) =>
							{
								logger.LogError($"Reintentando debido a {exception} {span}");
							});
					retry.Execute(() => CallSeeder(seeder, context, services));
					logger.LogInformation($"Migración completada: {typeof(TContext).Name}");
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"Se produjo un error al migrar la base de datos: {typeof(TContext).Name}");
				}
			}
			return host;
		}

		private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext? context, IServiceProvider services) where TContext : DbContext
		{
			context.Database.Migrate();
			seeder(context, services);
		}
	}
}