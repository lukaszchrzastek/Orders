using Orders.Domain.Enums;
using Orders.Domain.Models;

namespace Orders.Domain.Repositories
{
	public interface IEmailRepository
	{
		Task<bool> ExistsAsync(uint emailId);

		Task<Email?> GetByIdAsync(uint emailId, CancellationToken cancellationToken = default);
		Task<List<Email>> GetByIdsAsync(List<uint> emailIds, CancellationToken cancellationToken = default);
		Task<IEnumerable<Email>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<Email>> GetNewAsync(CancellationToken cancellationToken = default);		
		Task AddAsync(Email email, CancellationToken cancellationToken = default);		
		Task UpdateStatusAsync(uint emailId, EmailStatus newStatus, CancellationToken cancellationToken = default);
	}
}