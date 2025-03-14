using Ecommerce.Identity.Manager.Models;

namespace Ecommerce.Identity.Manager.Token
{
    public interface IJWTGenerator
    {
        string CreateToken(User user);
    }
}
