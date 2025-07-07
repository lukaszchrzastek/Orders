using Orders.Domain.ValueObjects;

namespace Orders.Domain.Models
{
	public class OrderLine
	{
		public ProductName ProductName { get; private set; }
		public Quantity Quantity { get; private set; }
		public Money Price { get; private set; }

		private OrderLine() { }

		public static OrderLine Create(ProductName productName, Quantity quantity, Money price)
		{
			return new OrderLine
			{
				ProductName = productName,
				Quantity = quantity,
				Price = price
			};			
		}
	}
}