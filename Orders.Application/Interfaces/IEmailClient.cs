using Orders.Domain.Models;

namespace Orders.Application.Interfaces
{
	public interface IEmailClient
	{
		Task<IEnumerable<Email>> FetchUnreadEmailsAsync(CancellationToken cancellationToken = default);

		Task MarkEmailAsReadAsync(List<uint> emailId, CancellationToken cancellationToken = default);
	}
}