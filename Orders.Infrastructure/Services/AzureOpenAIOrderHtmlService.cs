namespace Orders.Infrastructure.Services;

using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using Orders.Application.Interfaces;
using Orders.Application.Results;
using Orders.Domain.Models;
using Orders.Domain.ValueObjects;
using Orders.Infrastructure.Prompts;
using Orders.Infrastructure.Serialization;
using Orders.Infrastructure.Settings;
using Orders.Infrastructure.Utilities;
using System.Text.Json;

public class AzureOpenAIOrderHtmlService(
	IOptions<AzureOpenAISettings> settings,
	ILogger<AzureOpenAIOrderHtmlService> logger
	) : IOrderHtmlParserService
{
	private readonly AzureOpenAISettings _settings = settings.Value;
	private readonly ChatCompletionOptions _chatOptions = new()
	{
		MaxOutputTokenCount = 4096,
		Temperature = 0.2f,
		TopP = 1.0f
	};

	public async Task<OrderHtmlParserResult> ParseAsync(string html, CancellationToken cancellationToken = default)
	{
		try
		{
			var azureClient = new AzureOpenAIClient(
				new Uri(_settings.Endpoint),
				new AzureKeyCredential(_settings.ApiKey));

			var chatClient = azureClient.GetChatClient(_settings.Deployment);

			var messages = new List<ChatMessage>
			{
				new SystemChatMessage(PromptTemplates.OrderExtraction),
				new UserChatMessage(html)
			};

			var response = await chatClient.CompleteChatAsync(messages, _chatOptions, cancellationToken);
			var output = response.Value?.Content?.FirstOrDefault()?.Text;

			if (string.IsNullOrWhiteSpace(output) || !OrderJsonValidator.IsValid(output))
				return new OrderHtmlParserResult { Success = false };

			var parsed = JsonSerializer.Deserialize<OrderJsonDto>(output, JsonDefaults.CaseInsensitive);

			if (parsed is null)
				return new OrderHtmlParserResult { Success = false };

			var order = MapToOrder(parsed);

			return new OrderHtmlParserResult
			{
				Success = true,
				Order = order
			};
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to parse order from HTML");
			return new OrderHtmlParserResult
			{
				Success = false,
				ErrorMessage = ex.Message
			};
		}
	}

	private static Order MapToOrder(OrderJsonDto dto)
	{
		var lines = dto.Products.Select(p =>
			OrderLine.Create(
				ProductName.Create(p.ProductName),
				Quantity.Create(p.Quantity),
				Money.Create(p.Price, "zł")
			)).ToList();

		return Order.Create(OrderId.Create(dto.Id), lines);
	}
}
