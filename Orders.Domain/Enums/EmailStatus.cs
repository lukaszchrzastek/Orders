namespace Orders.Domain.Enums
{
	public enum EmailStatus
	{
		New = 1,
		Processed,
		ProcessedNoAttachments,
		ParsingFailed,
		Failed
	}
}
