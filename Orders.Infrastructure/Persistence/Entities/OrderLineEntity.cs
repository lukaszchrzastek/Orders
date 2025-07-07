namespace Orders.Infrastructure.Persistence.Entities
{
	public class OrderLineEntity
	{
		public int Id { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public int OrderEntityId { get; set; }
		public OrderEntity Order { get; set; }
	}
}