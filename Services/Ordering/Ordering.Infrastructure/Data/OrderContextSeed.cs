using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
	public class OrderContextSeed
	{
		public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
		{
			if (!orderContext.Orders.Any())
			{
				orderContext.Orders.AddRange(GetOrders());
				await orderContext.SaveChangesAsync();
				logger.LogInformation($"Base de datos de Orden: {typeof(OrderContext).Name} inicializada!!!");
			}
		}

		private static IEnumerable<Order> GetOrders()
		{
			return new List<Order>
			{
				new()
				{
					UserName = "kevin",
					FirstName = "Kevin",
					LastName = "Rodríguez",
					EmailAddress = "kevinr142@gmail.com",
					AddressLine = "Heredia",
					Country = "Costa Rica",
					TotalPrice = 750,
					State = "CR",
					ZipCode = "40701",

					CardName = "Visa",
					CardNumber = "1234567890123456",
					CreatedBy = "Kevin",
					Expiration = "12/25",
					Cvv = "123",
					PaymentMethod = 1,
					LastModifiedBy = "Kevin",
					LastModifiedDate = new DateTime(),
				}
			};
		}
	}
}