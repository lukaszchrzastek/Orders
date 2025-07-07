using Orders.Application.DTO;

namespace Orders.Application.DTO
{
	public class OrderJsonDto
	{
		public int Id { get; set; }
		public List<ProductJsonDto> Products { get; set; } = [];
	}
}
