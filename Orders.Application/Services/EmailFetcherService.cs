using Orders.Application.Interfaces;
using Orders.Domain.Services;
using System.Transactions;

namespace Orders.Application.Services;

public class EmailFetcherService(
	IEmailClient emailClient,
	IEmailService emailService,
	IEmailUniquenessChecker emailUniquenessChecker
	) : IEmailFetcherService
{		
	public async Task FetchUnreadEmailsAsync(CancellationToken cancellationToken)
	{
		var emials = await emailClient.FetchUnreadEmailsAsync(cancellationToken);

		if (!emials.Any())
			return;

		var emailIds = emials.Select(m => m.EmailId).ToList();
			
		var duplicateIds = await emailUniquenessChecker.GetDuplicateIdsAsync(emailIds);

		using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

		foreach (var emial in emials.Where(m => !duplicateIds.Contains(m.EmailId)))
		{				
			await emailService.AddEmailAsync(emial);
		}

		await emailClient.MarkEmailAsReadAsync(
			[.. emials.Select(m => m.EmailId)],
			cancellationToken);

		scope.Complete();
	}
}

