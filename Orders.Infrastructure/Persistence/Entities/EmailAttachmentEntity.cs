namespace Orders.Infrastructure.Persistence.Entities
{
	public class EmailAttachmentEntity
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public byte[] Content { get; set; }		
		public string ContentType { get; set; }
		public int EmailEntityId { get; set; }
		public EmailEntity Email { get; set; }
	}
}
