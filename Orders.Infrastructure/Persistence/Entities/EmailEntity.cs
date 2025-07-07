namespace Orders.Infrastructure.Persistence.Entities
{
	public enum EmailStatus
	{
		New = 1,
		Processed,
		ProcessedNoAttachments,
		ParsingFailed,
		Failed
	}

	public class EmailEntity
	{
		public int Id { get; set; }
		public uint EmailId { get; set; }
		public string Subject { get; set; } = string.Empty;
		public string From { get; set; } = string.Empty;
		public string Body { get; set; } = string.Empty;
		public DateTime ReceivedAt { get; set; }
		public DateTime ProcessedAt { get; set; }
		public List<EmailAttachmentEntity>? Attachments { get; set; }
		public EmailStatus Status { get; set; }
	}
}
