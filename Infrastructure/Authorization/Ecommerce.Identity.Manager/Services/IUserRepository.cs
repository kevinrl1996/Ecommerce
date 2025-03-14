using Ecommerce.Identity.Manager.DTOs;

namespace Ecommerce.Identity.Manager.Services
{
    public interface IUserRepository
    {
		Task<UserResponseDto> GetUser();
		Task<UserResponseDto> Login(UserLoginRequestDto userLoginRequestDto);
		Task<UserResponseDto> RegisterUser(UserRegisterRequestDto userRegisterRequestDto);
	}
}
