using Orders.Application.DTO;
using Orders.Application.Interfaces;

namespace Orders.Infrastructure.Notifiers
{
	public class OrderUpdateNotifier : IOrderUpdateNotifier
	{
		public event Action<List<OrderDto>>? OrdersUpdated;


		public void NotifyOrdersUpdated(List<OrderDto> orders)
		{
			OrdersUpdated?.Invoke(orders);
		}
	}
}