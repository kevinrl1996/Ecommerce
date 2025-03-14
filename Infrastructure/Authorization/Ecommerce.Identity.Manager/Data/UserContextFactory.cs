using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ecommerce.Identity.Manager.Data
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
	{
		public UserContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
			//optionsBuilder.UseSqlServer("Data Source=IdentityDb");
			optionsBuilder.UseMySQL("Data Source=IdentityDb");
			return new UserContext(optionsBuilder.Options);
		}
	}
}
