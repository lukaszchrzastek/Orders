namespace Orders.Infrastructure.Services;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Orders.Application.Interfaces;
using Orders.Domain.Models;
using Orders.Infrastructure.Settings;

public class ImapEmailClient(
	IOptions<ImapSettings> options,
	ILogger<ImapEmailClient> logger,
	TimeProvider timeProvider) : IEmailClient
{
	private readonly ImapSettings _settings = options.Value;
	private readonly ILogger<ImapEmailClient> _logger = logger;
	private readonly TimeProvider _timeProvider = timeProvider;

	public async Task<IEnumerable<Email>> FetchUnreadEmailsAsync(CancellationToken cancellationToken = default)
	{
		var messages = new List<Email>();

		await WithConnectedClientAsync(async client =>
		{
			var uids = await client.Inbox.SearchAsync(SearchQuery.NotSeen, cancellationToken);

			foreach (var uid in uids)
			{
				var email = await ConvertToEmailAsync(client.Inbox, uid, cancellationToken);				
				messages.Add(email);
			}
		}, FolderAccess.ReadOnly, cancellationToken);

		return messages;
	}

	public async Task MarkEmailAsReadAsync(List<uint> emailIds, CancellationToken cancellationToken = default)
	{
		await WithConnectedClientAsync(async client =>
		{
			var uids = emailIds.Select(id => new UniqueId(id)).ToList();
			await client.Inbox.AddFlagsAsync(uids, MessageFlags.Seen, true, cancellationToken);
		}, FolderAccess.ReadWrite, cancellationToken);
	}

	private async Task WithConnectedClientAsync(
		Func<ImapClient, Task> action,
		FolderAccess access,
		CancellationToken cancellationToken)
	{
		using var client = new ImapClient();
		await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, cancellationToken);
		await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
		await client.Inbox.OpenAsync(access, cancellationToken);

		try
		{
			await action(client);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unexpected error while handling IMAP: {Message}", ex.Message);
		}
		finally
		{
			await client.DisconnectAsync(true, cancellationToken);
		}
	}

	private async Task<Email> ConvertToEmailAsync(
		IMailFolder inbox,
		UniqueId uid,
		CancellationToken cancellationToken)
	{
		var message = await inbox.GetMessageAsync(uid, cancellationToken);
		var attachments = await GetEmailAttachmentsAsync(message, cancellationToken);
		
		return Email.Create(		
			uid.Id,
			message.Subject,
			message.From.ToString(),
			message.TextBody ?? message.HtmlBody ?? string.Empty,
			_timeProvider.GetLocalNow().UtcDateTime,
			Domain.Enums.EmailStatus.New,
			[.. attachments]
		);
	}

	private static async Task<IEnumerable<EmailAttachment>> GetEmailAttachmentsAsync(
		MimeMessage message,
		CancellationToken cancellationToken = default)
	{
		var attachments = new List<EmailAttachment>();

		foreach (var attachment in message.Attachments)
		{
			var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType?.Name ?? "file";
			var contentType = attachment.ContentType?.MimeType ?? "application/octet-stream";
			await using var memory = new MemoryStream();

			switch (attachment)
			{
				case MessagePart msgPart:
					msgPart.Message.WriteTo(memory, cancellationToken);
					break;
				case MimePart mimePart:
					await mimePart.Content.DecodeToAsync(memory, cancellationToken);
					break;
			}

			attachments.Add(EmailAttachment.Create(			
				fileName,
				memory.ToArray(),
				contentType
			));
		}

		return attachments;
	}
}
