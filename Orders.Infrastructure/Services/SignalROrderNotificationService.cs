using Microsoft.AspNetCore.SignalR;
using Orders.Application.DTO;
using Orders.Application.Interfaces;
using Orders.Domain.Models;
using Orders.Infrastructure.Hubs;
using Orders.Infrastructure.Persistence.Mappers;

namespace Orders.Infrastructure.Services;

public class SignalROrderNotificationService(IHubContext<OrderHub> hubContext) : IOrderNotificationService
{
	private readonly IHubContext<OrderHub> _hubContext = hubContext;

	public Task NotifyOrderUpdatedAsync(Order order, CancellationToken cancellationToken)
	{			
		return _hubContext.Clients.All.SendAsync("OrderUpdated", new List<OrderDto> { OrderMapper.MapToDto(order) }, cancellationToken);
	}
}
