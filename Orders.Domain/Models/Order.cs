using Orders.Domain.ValueObjects;

namespace Orders.Domain.Models
{
	public class Order
	{
		public OrderId Id { get; private set; }

		private List<OrderLine> _orderLines = [];
		public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

		private Order() { }

		public static Order Create(OrderId id, List<OrderLine> orderLines)
		{
			return new Order
			{
				Id = id,
				_orderLines = orderLines ?? throw new ArgumentNullException(nameof(orderLines))
			};
		}
	}
}