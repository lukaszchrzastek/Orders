using Orders.Domain.Models;
using Orders.Domain.Repositories;

namespace Orders.Domain.Services
{
	public class OrderService(IOrderRepository orderRepository) : IOrderService
	{		
		public async Task AddOrderAsync(Order order)
		{
			ArgumentNullException.ThrowIfNull(order);

			if ( await orderRepository.ExistsAsync(order.Id.Value))
			{
				throw new InvalidOperationException($"Order with ID {order.Id.Value} already exists.");
			}

			await orderRepository.AddAsync(order);
		}

		public Task<bool> ExistsAsync(int orderId)
		{
			return orderRepository.ExistsAsync(orderId);
		}
	}
}