using Orders.Domain.Enums;
using Orders.Domain.Models;

namespace Orders.Domain.Services
{
	public class EmailProcessingRules: IEmailProcessingRules
	{
		public EmailStatus DetermineStatus(Email email, string? html)
		{
			if (email.Attachments is null || email.Attachments.Count == 0)
				return EmailStatus.ProcessedNoAttachments;

			if (email.Attachments.Count != 1)
				return EmailStatus.Failed;

			if (string.IsNullOrEmpty(html))
				return EmailStatus.Failed;

			return EmailStatus.Processed;
		}
	}
}
