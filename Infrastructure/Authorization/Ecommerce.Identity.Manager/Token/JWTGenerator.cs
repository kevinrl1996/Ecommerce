using Ecommerce.Identity.Manager.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Identity.Manager.Token
{
	public class JWTGenerator : IJWTGenerator
	{
		private readonly IConfiguration _configuration;

		public JWTGenerator(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string CreateToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.NameId, user.UserName!)
			};

			//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTKey:Secret")!));
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ecawiasqrpqrgyhwnolrudpbsrwaynbqdayndnmcehjnwqyouikpodzaqxivwkconwqbhrmxfgccbxbyljguwlxhdlcvxlutbnwjlgpfhjgqbegtbxbvwnacyqnltrby"));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescription = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(30),
				SigningCredentials = credentials
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescription);
			return tokenHandler.WriteToken(token);
		}
	}
}
