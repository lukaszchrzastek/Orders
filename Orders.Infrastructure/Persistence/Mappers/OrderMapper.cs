using Orders.Application.DTO;
using Orders.Domain.Models;
using Orders.Domain.ValueObjects;
using Orders.Infrastructure.Persistence.Entities;

namespace Orders.Infrastructure.Persistence.Mappers
{
	public static class OrderMapper
	{
		public static Order ToDomain(OrderEntity entity)
		{			
			var orderLines = entity.OrderLines?.Select(OrderLineMapper.ToDomain).ToList() ?? [];			
			var order = Order.Create(OrderId.Create(entity.EmailOrderId), orderLines);
			return order;
		}

		public static OrderEntity ToEntity(Order domain) =>
			new()
			{
				EmailOrderId = domain.Id.Value,
				OrderLines = domain.OrderLines?.Select(OrderLineMapper.ToEntity).ToList() ?? []
			};

		public static OrderDto MapToDto(Order domain) => new()
		{
			Id = domain.Id.Value,
			OrderLines = domain.OrderLines?.Select(OrderLineMapper.ToDto).ToList() ?? []
		};
	}
}