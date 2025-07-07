namespace Orders.Infrastructure.Persistence.Entities
{
	public class OrderEntity
	{
		public int Id { get; set; }
		public int EmailOrderId { get; set; }
		public List<OrderLineEntity> OrderLines { get; set; } = [];
	}
}