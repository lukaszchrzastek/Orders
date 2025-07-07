using Orders.Domain.Models;

namespace Orders.Domain.Services
{
	public interface IOrderService
	{
		Task AddOrderAsync(Order order);

		Task<bool> ExistsAsync(int orderId);		
	}
}