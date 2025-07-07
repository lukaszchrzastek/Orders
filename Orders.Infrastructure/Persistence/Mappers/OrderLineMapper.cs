using Orders.Application.DTO;
using Orders.Domain.Models;
using Orders.Domain.ValueObjects;
using Orders.Infrastructure.Persistence.Entities;

namespace Orders.Infrastructure.Persistence.Mappers
{
	public static class OrderLineMapper
	{
		public static OrderLine ToDomain(OrderLineEntity entity)
		{
			return OrderLine.Create(
				ProductName.Create(entity.ProductName),
				Quantity.Create(entity.Quantity),
				Money.Create(entity.Price, "zł")
			);			
		}			

		public static OrderLineEntity ToEntity(OrderLine domain) =>
			new()
			{
				ProductName = domain.ProductName.Value,
				Quantity = domain.Quantity.Value,
				Price = domain.Price.Amount
			};

		public static OrderLineDto ToDto(OrderLine orderLine) =>
			new()
			{
				ProductName = orderLine.ProductName.Value,
				Quantity = orderLine.Quantity.Value,
				Price = orderLine.Price.Amount
			};
	}
}