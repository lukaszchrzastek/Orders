using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Orders.Infrastructure.Hubs;

public class OrderHub(ILogger<Hub> logger) : Hub
{
	public async Task NotifyNewOrder()
	{
		await Clients.All.SendAsync("OrderUpdated");
	}
		
	public override async Task OnConnectedAsync()
	{
		logger.LogInformation("Connected: {ConnectionId}", Context.ConnectionId);
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		logger.LogInformation("Disconnected: {ConnectionId}", Context.ConnectionId);
		await base.OnDisconnectedAsync(exception);
	}		
}
