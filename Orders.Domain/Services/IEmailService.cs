using Orders.Domain.Models;

namespace Orders.Domain.Services
{
	public interface IEmailService
	{
		Task AddEmailAsync(Email email);
	}
}
