using Ecommerce.Identity.Manager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Identity.Manager.Data
{
	public class UserContextSeed
	{
		/*public static async Task InsertarData(AppDbContext context, UserManager<User> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new User
				{
					Name = "Kevin",
					LastName = "Rodríguez",
					Email = "kevinr142@gmail.com",
					UserName = "kevinrl_cr",
					Phone = "12345678"
				};

				await userManager.CreateAsync(user, "Zaqwsxcde-1234");
			}

			context.SaveChanges();
		}*/
		public static async Task SeedAsync(UserContext userContext, UserManager<User> userManager, ILogger<UserContextSeed> logger)
		{
			if (!userContext.Users.Any())
			{
				var user = new User
				{
					Name = "Kevin",
					LastName = "Rodríguez",
					Email = "kevinr142@gmail.com",
					UserName = "kevinrl_cr",
					Phone = "12345678"
				};

				//userContext.Users.AddRange(GetUsers());
				await userManager.CreateAsync(user, "Zaqwsxcde-1234");
				await userContext.SaveChangesAsync();
				logger.LogInformation($"Base de datos de usuarios inicializada!!!");
			}
		}

		/*private static IEnumerable<User> GetUsers()
		{
			return new List<User>
			{
				new()
				{
					Name = "Kevin",
					LastName = "Rodríguez",
					Email = "kevinr142@gmail.com",
					UserName = "kevinrl_cr",
					Phone = "12345678"
				}
			};
		}*/
	}
}
