namespace Ordering.Infrastructure.Exceptions
{
	public class ValidationException : ApplicationException
	{
		public Dictionary<string, string[]> Errors { get; }
		public ValidationException() : base("Se produjeron uno o más errores de validación.")
		{
			Errors = new Dictionary<string, string[]>();
		}

		public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> failures) : this()
		{
			Errors = failures
					.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
					.ToDictionary(failure => failure.Key, failure => failure.ToArray());
		}
	}
}