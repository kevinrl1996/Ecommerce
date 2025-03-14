using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ecommerce.Identity.Manager.Configurations
{
	public static class CustomJwtAuthExtension
	{
		public static void AddCustomJwtAuthentication(this IServiceCollection services)
		{
			//using var serviceProvider = services.BuildServiceProvider();
			//var configuration = serviceProvider.GetRequiredService<IConfiguration>();

			//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWTKey:Secret")!));
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ecawiasqrpqrgyhwnolrudpbsrwaynbqdayndnmcehjnwqyouikpodzaqxivwkconwqbhrmxfgccbxbyljguwlxhdlcvxlutbnwjlgpfhjgqbegtbxbvwnacyqnltrby"));

			//services.AddAuthentication(o =>
			//{
			//	o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			//	o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			//}).AddJwtBearer(o =>
			//{
			//	o.RequireHttpsMetadata = false;
			//	o.SaveToken = true;
			//	o.TokenValidationParameters = new TokenValidationParameters
			//	{
			//		ValidateIssuerSigningKey = true,
			//		ValidateIssuer = false,
			//		ValidateAudience = false,
			//		IssuerSigningKey = key
			//		//IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTGenerator..JWT_SECURITY_KEY))
			//	};
			//});
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opt =>
				{
					opt.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = key,
						ValidateAudience = false,
						ValidateIssuer = false
					};
				});
		}
	}
}
