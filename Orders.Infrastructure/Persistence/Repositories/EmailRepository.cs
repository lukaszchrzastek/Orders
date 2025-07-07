using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Orders.Domain.Enums;
using Orders.Domain.Models;
using Orders.Domain.Repositories;
using Orders.Infrastructure.Persistence.Mappers;

namespace Orders.Infrastructure.Persistence.Repositories
{
	public class EmailRepository(AppDbContext context) : IEmailRepository
	{
		private readonly AppDbContext _context = context;
		             
		public async Task<bool> ExistsAsync(uint emailId)
		{
			return await _context.Emails.AnyAsync(o => o.EmailId == emailId);
		}

		public async Task<Email?> GetByIdAsync(uint emailId, CancellationToken cancellationToken = default)
		{
			var entity = await _context.Emails
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == emailId, cancellationToken);

			return entity is null ? null : EmailMapper.ToDomain(entity);
		}

		public async Task<List<Email>> GetByIdsAsync(List<uint> emailIds, CancellationToken cancellationToken = default)
		{

			if (emailIds is null || emailIds.Count == 0)
				return [];

			var parameters = emailIds
				.Select((id, index) => new MySqlParameter($"@p{index}", id))
				.ToArray();

			var inClause = string.Join(", ", parameters.Select(p => p.ParameterName));

			var sql = $"SELECT * FROM Emails WHERE EmailId IN ({inClause})";

			var entities = await _context.Emails
				.FromSqlRaw(sql, parameters)
				.ToListAsync(cancellationToken);

			return entities.Select(EmailMapper.ToDomain).ToList();
		}

		public async Task AddAsync(Email email, CancellationToken cancellationToken = default)
		{
			_context.Emails.Add(EmailMapper.ToEntity(email));
			await _context.SaveChangesAsync(cancellationToken);
		}

		public async Task<IEnumerable<Email>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			var emails = await _context.Emails
				.AsNoTracking()
				.OrderByDescending(e => e.ReceivedAt)
				.ToListAsync(cancellationToken);

			return emails.Select(e => EmailMapper.ToDomain(e));
		}

		public async Task<IEnumerable<Email>> GetNewAsync(CancellationToken cancellationToken = default)
		{
			return await _context.Emails
				.Include(e => e.Attachments)
				.AsNoTracking()
				.Where(e => e.Status == Entities.EmailStatus.New)
				.OrderByDescending(e => e.ReceivedAt)
				.Select(e => EmailMapper.ToDomain(e))
				.ToListAsync(cancellationToken);
		}

		public async Task UpdateStatusAsync(uint emailId, EmailStatus newStatus, CancellationToken cancellationToken = default)
		{
			var entity = await _context.Emails
				.FirstOrDefaultAsync(e => e.EmailId == emailId, cancellationToken)
				?? throw new InvalidOperationException($"Email o ID {emailId} nie został znaleziony.");
			
			entity.Status = (Entities.EmailStatus)(int)newStatus;
			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}