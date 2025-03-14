using Ecommerce.Identity.Manager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Identity.Manager.Data
{
	public class UserContext : IdentityDbContext<User>
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options) { }

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			foreach (var entry in ChangeTracker.Entries<EntityBase>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.CreatedDate = DateTime.Now;
						entry.Entity.CreatedBy = "kevin"; //TODO: Replace with auth server
						break;
					case EntityState.Modified:
						entry.Entity.LastModifiedDate = DateTime.Now;
						entry.Entity.LastModifiedBy = "kevin"; //TODO: Replace with auth server
						break;
				}
			}

			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
	}
}
