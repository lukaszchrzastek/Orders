using Orders.Domain.Models;
using Orders.Domain.Repositories;

namespace Orders.Domain.Services
{
	public class EmailService(IEmailRepository emailRepository) : IEmailService
	{		
		public async Task AddEmailAsync(Email email)
		{
			ArgumentNullException.ThrowIfNull(email);

			if ( await emailRepository.ExistsAsync(email.EmailId))
			{
				throw new InvalidOperationException($"Email with ID {email.EmailId} already exists.");
			}

			await emailRepository.AddAsync(email);
		}
	}
}