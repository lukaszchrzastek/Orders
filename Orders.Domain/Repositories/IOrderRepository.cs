using Orders.Domain.Models;

namespace Orders.Domain.Repositories
{
	public interface IOrderRepository
	{
		Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
		Task AddAsync(Order order, CancellationToken cancellationToken = default);
		Task<bool> ExistsAsync(int orderId);
	}
}