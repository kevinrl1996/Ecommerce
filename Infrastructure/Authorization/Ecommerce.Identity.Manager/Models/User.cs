using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Identity.Manager.Models
{
	public class User : IdentityUser
	{
		public string? Name { get; set; }
		public string? LastName { get; set; }
		public string? Phone { get; set; }
	}
}
