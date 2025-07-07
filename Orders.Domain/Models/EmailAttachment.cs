namespace Orders.Domain.Models
{
	public class EmailAttachment
	{
		public string FileName { get; private set; }
		public byte[] Content { get; private set; }		
		public string ContentType { get; private set; }

		private EmailAttachment() { }

		public static EmailAttachment Create(
			string fileName,
			byte[] content,
			string contentType)
		{
			return new EmailAttachment
			{
				FileName = fileName,
				Content = content,
				ContentType = contentType
			};
		}
	}
}
