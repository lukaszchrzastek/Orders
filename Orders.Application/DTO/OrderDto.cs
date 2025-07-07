namespace Orders.Application.DTO
{
	public class OrderDto
	{
		public int Id { get; init; }
		public List<OrderLineDto> OrderLines { get; init; } = [];
	}
}
