using Ecommerce.Identity.Extensions;
using Ecommerce.Identity.Manager.Data;
using Ecommerce.Identity.Manager.Middlewares;
using Ecommerce.Identity.Manager.Models;
using Ecommerce.Identity.Manager.Services;
using Ecommerce.Identity.Manager.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(
	builder.Configuration.GetConnectionString("IdentityConnectionString"),
	sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));*/
builder.Services.AddDbContext<UserContext>(options =>
	options.UseMySQL(builder.Configuration.GetConnectionString("IdentityConnectionString")));

// Add services to the container.

builder.Services.AddControllers(opt =>
{
	var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
	opt.Filters.Add(new AuthorizeFilter(policy));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var builderSecurity = builder.Services.AddIdentityCore<User>();

var identityBuilder = new IdentityBuilder(builderSecurity.UserType, builder.Services);
identityBuilder.AddEntityFrameworkStores<UserContext>();
identityBuilder.AddSignInManager<SignInManager<User>>();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IJWTGenerator, JWTGenerator>();
builder.Services.AddScoped<IUserSession, UserSession>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

/*var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opt =>
				{
					opt.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = key,
						ValidateAudience = false,
						ValidateIssuer = false
					};
				});*/

var app = builder.Build();

//Apply db migration
app.MigrateDatabase<UserContext>((context, services) =>
{
	var userManager = services.GetRequiredService<UserManager<User>>();
	var logger = services.GetService<ILogger<UserContextSeed>>();
	UserContextSeed.SeedAsync(context, userManager, logger).Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<MiddlewareManager>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
