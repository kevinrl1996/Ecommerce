using Ecommerce.Identity.Models;

namespace Ecommerce.Identity.Services
{
	public interface IAuthService
	{
		Task<(int, string)> Registration(RegisterModel model, string role);
		Task<(int, string)> Login(LoginModel model);
	}
}
