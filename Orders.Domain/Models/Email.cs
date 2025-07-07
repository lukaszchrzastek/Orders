using Orders.Domain.Enums;

namespace Orders.Domain.Models
{
	public class Email
	{
		public uint EmailId { get; private set; }
		public string Subject { get; private set; }
		public string From { get; private set; }
		public string Body { get; private set; }
		public DateTime ReceivedAt { get; private set; }
		public List<EmailAttachment>? Attachments { get; private set; }
		public EmailStatus Status { get; private set; }

		private Email() { }

		public void SetStatus(EmailStatus status)
		{			
			Status = status;
		}

		public static Email Create(
			uint emailId,
			string subject,
			string from,
			string body,
			DateTime receivedAt,
			EmailStatus status,
			List<EmailAttachment>? attachments)
		{
			return new Email
			{
				EmailId = emailId,
				Subject = subject,
				From = from,
				Body = body,
				ReceivedAt = receivedAt,
				Status = status,
				Attachments = attachments
			};
		}

	}
}
