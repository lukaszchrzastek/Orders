using Microsoft.AspNetCore.Mvc;
using Orders.Application.DTO;
using Orders.Domain.Repositories;

namespace Orders.Presentation.Blazor;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderRepository orderRepository) : ControllerBase
{	

	[HttpGet]
	public async Task<IActionResult> Get()
	{		
		var entities = await orderRepository.GetAllAsync();
		var orders = entities.Select(e => new OrderDto
			{
				Id = e.Id.Value,
				OrderLines = [.. e.OrderLines.Select(p => new OrderLineDto
				{
					ProductName = p.ProductName.Value,
					Quantity = p.Quantity.Value,
					Price = p.Price.Amount
				})]
			}).ToList();

		return Ok(orders);
	}
}