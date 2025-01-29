using FluentValidation;

namespace Ordering.Infrastructure.Behaviour
{
	public class ValidationBehaviour<TRequest> where TRequest : class
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		// Método para validar y ejecutar la acción siguiente
		public async Task ValidateAndExecuteAsync(TRequest request, Func<Task> next)
		{
			if (_validators.Any())
			{
				var context = new ValidationContext<TRequest>(request);
				// Ejecutamos todas las validaciones
				var validationResults = await Task.WhenAll(
					_validators.Select(v => v.ValidateAsync(context)));

				// Recogemos los errores de validación
				var failures = validationResults.SelectMany(e => e.Errors).Where(f => f != null).ToList();

				// Si hay errores, lanzamos una excepción
				if (failures.Count != 0)
				{
					throw new ValidationException(failures);
				}
			}

			// Ejecutamos la lógica posterior
			await next();
		}
	}
}
