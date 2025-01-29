using Ordering.Core.Entities;

namespace Ordering.Core.Interfaces
{
	public interface IOrderRepository : IAsyncRepository<Order>
	{
		Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
	}
}