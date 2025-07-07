using Orders.Domain.Models;

namespace Orders.Application.Interfaces;

public interface IOrderNotificationService
{
	Task NotifyOrderUpdatedAsync(Order order, CancellationToken cancellationToken);
}
