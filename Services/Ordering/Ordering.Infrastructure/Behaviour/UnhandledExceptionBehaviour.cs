using Microsoft.Extensions.Logging;

namespace Ordering.Infrastructure.Behaviour
{
	public class UnhandledExceptionHandler<TRequest, TResponse>
	{
		private readonly ILogger<UnhandledExceptionHandler<TRequest, TResponse>> _logger;

		public UnhandledExceptionHandler(ILogger<UnhandledExceptionHandler<TRequest, TResponse>> logger)
		{
			_logger = logger;
		}

		public async Task<TResponse> HandleRequest(TRequest request, Func<Task<TResponse>> next)
		{
			try
			{
				return await next();
			}
			catch (Exception ex)
			{
				var requestName = typeof(TRequest).Name;
				_logger.LogError(ex, $"Se produjo una excepción no controlada con el nombre de la solicitud: {requestName}, {request}");
				throw;
			}
		}
	}
}
