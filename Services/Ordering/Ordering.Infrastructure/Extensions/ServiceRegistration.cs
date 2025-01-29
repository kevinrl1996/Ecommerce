using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Behaviour;
using System.Reflection;

namespace Ordering.Infrastructure.Extensions
{
	public static class ServiceRegistration
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddTransient(typeof(ValidationBehaviour<>));
			services.AddTransient(typeof(UnhandledExceptionHandler<,>));
			return services;
		}
	}
}