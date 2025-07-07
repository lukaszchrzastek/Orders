using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Orders.Domain.Models;
using Orders.Domain.Repositories;
using Orders.Infrastructure.Persistence.Mappers;

namespace Orders.Infrastructure.Persistence.Repositories
{
	public class OrderRepository(AppDbContext context) : IOrderRepository
	{
		private readonly AppDbContext _context = context;

		public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
		{
			_context.Orders.Add(OrderMapper.ToEntity(order));
			await _context.SaveChangesAsync(cancellationToken);
		}

		public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			var orders = await _context.Orders
				.AsNoTracking()
				.Include(o => o.OrderLines)
				.OrderByDescending(e => e.Id)
				.ToListAsync(cancellationToken);
			return [.. orders.Select(o => OrderMapper.ToDomain(o))];
		}

		public async Task<bool> ExistsAsync(int orderId)
		{
			return await _context.Orders.AnyAsync(o => o.EmailOrderId == orderId);
		}

		public async Task<List<Order>> GetByIdsAsync(List<int> orderIds, CancellationToken cancellationToken = default)
		{
			if (orderIds is null || orderIds.Count == 0)
				return [];

			var parameters = orderIds
				.Select((id, index) => new MySqlParameter($"@p{index}", id))
				.ToArray();

			var inClause = string.Join(", ", parameters.Select(p => p.ParameterName));

			var sql = $"SELECT * FROM Orders WHERE EmailOrderId IN ({inClause})";

			var orders = await _context.Orders
				.FromSqlRaw(sql, parameters)
				.ToListAsync(cancellationToken);

			return [.. orders.Select(OrderMapper.ToDomain)];

		}
	}
}