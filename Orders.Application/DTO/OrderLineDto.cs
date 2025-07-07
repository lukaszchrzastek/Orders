namespace Orders.Application.DTO
{
	public class OrderLineDto
	{
		public required string ProductName { get; init; }
		public int Quantity { get; init; }
		public decimal Price { get; init; }
	}
}
