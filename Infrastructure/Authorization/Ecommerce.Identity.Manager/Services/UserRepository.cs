using Ecommerce.Identity.Manager.Data;
using Ecommerce.Identity.Manager.DTOs;
using Ecommerce.Identity.Manager.Middlewares;
using Ecommerce.Identity.Manager.Models;
using Ecommerce.Identity.Manager.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Ecommerce.Identity.Manager.Services
{
	public class UserRepository : IUserRepository
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IJWTGenerator _jWTGenerator;
		private readonly UserContext _context;
		private readonly IUserSession _userSession;

		public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, IJWTGenerator jWTGenerator, UserContext context, IUserSession userSession)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jWTGenerator = jWTGenerator;
			_context = context;
			_userSession = userSession;
		}

		private UserResponseDto TransformUserToUserDto(User user)
		{
			return new UserResponseDto
			{
				Id = user.Id,
				Name = user.Name,
				LastName = user.LastName,
				Phone = user.Phone,
				Email = user.Email,
				UserName = user.UserName,
				Token = _jWTGenerator.CreateToken(user)
			};
		}

		public async Task<UserResponseDto> GetUser()
		{
			var user = await _userManager.FindByNameAsync(_userSession.GetUserSession());

			if (user is null)
			{
				throw new MiddlewareException(
					HttpStatusCode.Unauthorized,
					new { mensaje = "El usuario del token no existe en la base de datos." }
				);
			}
			return TransformUserToUserDto(user!);
		}

		public async Task<UserResponseDto> Login(UserLoginRequestDto userLoginRequestDto)
		{
			var user = await _userManager.FindByEmailAsync(userLoginRequestDto.Email!);

			if (user is null)
			{
				throw new MiddlewareException(
					HttpStatusCode.Unauthorized,
					new { mensaje = "El email del usuario no existe en la base de datos." }
				);
			}

			var resultado = await _signInManager.CheckPasswordSignInAsync(user!, userLoginRequestDto.Password!, false);

			if (resultado.Succeeded)
			{
				return TransformUserToUserDto(user);
			}

			throw new MiddlewareException(
				 HttpStatusCode.Unauthorized,
				 new { mensaje = "Las credenciales son incorrectas." }
			);
		}

		public async Task<UserResponseDto> RegisterUser(UserRegisterRequestDto userRegisterRequestDto)
		{
			var existeEmail = await _context.Users.Where(x => x.Email == userRegisterRequestDto.Email).AnyAsync();

			if (existeEmail)
			{
				throw new MiddlewareException(
					HttpStatusCode.BadRequest,
					new { mensaje = "El email del usuario ya existe en la base de datos" }
				);
			}

			var existeUsername = await _context.Users.Where(x => x.UserName == userRegisterRequestDto.UserName).AnyAsync();

			if (existeUsername)
			{
				throw new MiddlewareException(
					HttpStatusCode.BadRequest,
					new { mensaje = "El username del usuario ya existe en la base de datos" }
				);
			}

			var user = new User
			{
				Name = userRegisterRequestDto.Name,
				LastName = userRegisterRequestDto.LastName,
				Phone = userRegisterRequestDto.Phone,
				Email = userRegisterRequestDto.Email,
				UserName = userRegisterRequestDto.UserName,
			};

			var result = await _userManager.CreateAsync(user!, userRegisterRequestDto.Password!);

			if (result.Succeeded)
			{
				return TransformUserToUserDto(user);
			}

			throw new MiddlewareException(
				HttpStatusCode.BadRequest,
				new { mensaje = result.Errors }
			);

			throw new Exception("No se pudo registrar el usuario.");
		}
	}
}
