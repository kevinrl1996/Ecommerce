using Ecommerce.Identity.Manager.DTOs;
using Ecommerce.Identity.Manager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Identity.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<ActionResult<UserResponseDto>> Login([FromBody] UserLoginRequestDto userLoginRequestDto)
		{
			return await _userRepository.Login(userLoginRequestDto);
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserRegisterRequestDto userRegisterRequestDto)
		{
			return await _userRepository.RegisterUser(userRegisterRequestDto);
		}

		[HttpGet]
		public async Task<ActionResult<UserResponseDto>> GetUser()
		{
			return await _userRepository.GetUser();
		}
	}
}
