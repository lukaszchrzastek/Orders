using Orders.Application.Interfaces;
using Orders.Domain.Enums;
using Orders.Domain.Models;
using Orders.Domain.Repositories;
using Orders.Domain.Services;

namespace Orders.Application.Services;

public class EmailProcessorService(
	IEmailRepository emailRepository,
	IOrderService orderService,
	IOrderNotificationService orderNotificationService,
	IOrderHtmlParserService parser,
	IEmailProcessingRules rules
	) : IEmailProcessorService
{
	private readonly IEmailRepository _emailRepository = emailRepository;
	private readonly IOrderService _orderService = orderService;
	private readonly IOrderNotificationService _orderNotificationService = orderNotificationService;
	private readonly IOrderHtmlParserService _parser = parser;
	private readonly IEmailProcessingRules _rules = rules;

	public async Task ProcessNewEmailsAsync(CancellationToken cancellationToken)
	{
		var emails = await _emailRepository.GetNewAsync(cancellationToken);

		foreach (var email in emails)
		{
			await ProcessEmailAsync(email, cancellationToken);
		}
	}

	private async Task ProcessEmailAsync(Email email, CancellationToken cancellationToken)
	{
		string? html = null;

		if (email.Attachments?.Count == 1)
		{
			html = await ExtractHtmlFromContentAsync(email.Attachments[0].Content, cancellationToken);
		}

		var status = _rules.DetermineStatus(email, html);

		if (status == EmailStatus.Processed && html is not null)
		{
			var result = await _parser.ParseAsync(html, cancellationToken);

			if (result.Success && result.Order is not null)
			{
				if (!await _orderService.ExistsAsync(result.Order.Id.Value))
				{
					await _orderService.AddOrderAsync(result.Order);

					await _orderNotificationService.NotifyOrderUpdatedAsync(result.Order, cancellationToken);
				}
			}
			else
			{
				status = EmailStatus.Failed;
			}
		}

		email.SetStatus(status);
		await _emailRepository.UpdateStatusAsync(email.EmailId, email.Status, cancellationToken);
	}

	private static async Task<string> ExtractHtmlFromContentAsync(byte[] content, CancellationToken ct)
	{
		await using var stream = new MemoryStream(content);
		using var reader = new StreamReader(stream);
		var raw = await reader.ReadToEndAsync(ct);
		var index = raw.IndexOf("<!DOCTYPE", StringComparison.OrdinalIgnoreCase);
		return index >= 0 ? raw[index..] : string.Empty;
	}
}

