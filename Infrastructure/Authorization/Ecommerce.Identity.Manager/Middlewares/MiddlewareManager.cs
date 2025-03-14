using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Ecommerce.Identity.Manager.Middlewares
{
	public class MiddlewareManager
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<MiddlewareManager> _logger;

		public MiddlewareManager(RequestDelegate next, ILogger<MiddlewareManager> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await ManagerExceptionAsync(context, ex, _logger);
			}
		}

		private async Task ManagerExceptionAsync(HttpContext context, Exception ex, ILogger<MiddlewareManager> logger)
		{
			object? errores = null;

			switch (ex)
			{
				case MiddlewareException me:
					logger.LogError(ex, "Middleware Error");
					errores = me.Errors;
					context.Response.StatusCode = (int)me.Code;
					break;

				case Exception e:
					logger.LogError(ex, "Error de Servidor");
					errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					break;
			}

			context.Response.ContentType = "application/json";
			var resultados = string.Empty;

			if (errores != null)
			{
				resultados = JsonConvert.SerializeObject(new { errores });
			}

			await context.Response.WriteAsync(resultados);
		}
	}
}
