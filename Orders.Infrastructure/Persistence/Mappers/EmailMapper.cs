namespace Orders.Infrastructure.Persistence.Mappers;

using Orders.Domain.Models;
using Orders.Infrastructure.Persistence.Entities;

public static class EmailMapper
{
	public static Email ToDomain(EmailEntity entity)
	{
		return Email.Create(
			entity.EmailId,
			entity.Subject,
			entity.From,
			entity.Body,
			entity.ReceivedAt,
			(Domain.Enums.EmailStatus)(int)entity.Status,
			entity.Attachments?
				.Select(EmailAttachmentMapper.ToDomain)
				.ToList() ?? []
			);
	}

	public static EmailEntity ToEntity(Email domain) =>
		new()
		{
			EmailId = domain.EmailId,
			Subject = domain.Subject,
			Body = domain.Body,
			ReceivedAt = domain.ReceivedAt,
			Attachments = domain.Attachments?
				.Select(EmailAttachmentMapper.ToEntity)
				.ToList() ?? [],
			Status = (Entities.EmailStatus)(int)domain.Status
		};
}
