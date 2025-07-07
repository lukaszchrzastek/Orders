namespace Orders.Infrastructure.Utilities
{
	public class OrderJsonDto
	{
		public int Id { get; set; }
		public List<ProductJsonDto> Products { get; set; } = [];
	}

	public class ProductJsonDto
	{
		public string ProductName { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}

}
