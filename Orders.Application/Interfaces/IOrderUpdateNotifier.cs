using Orders.Application.DTO;

namespace Orders.Application.Interfaces
{
	public interface IOrderUpdateNotifier
	{
		event Action<List<OrderDto>> OrdersUpdated;

		void NotifyOrdersUpdated(List<OrderDto> orders);
	}
}