namespace Orders.Infrastructure.Persistence.Mappers;
using Orders.Domain.Models;
using Orders.Infrastructure.Persistence.Entities;

public static class EmailAttachmentMapper
{
	public static EmailAttachment ToDomain(EmailAttachmentEntity entity)
	{
		return EmailAttachment.Create(entity.FileName, entity.Content, entity.ContentType);		
	}

	public static EmailAttachmentEntity ToEntity(EmailAttachment domain) =>
		new()
		{
			FileName = domain.FileName,
			Content = domain.Content,			
			ContentType = domain.ContentType
		};
}