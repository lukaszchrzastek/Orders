using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.Application.Interfaces;

namespace Orders.Infrastructure.Services;

public class EmailWorkerService : BackgroundService
{
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly IConfiguration _config;
	private readonly ILogger<EmailWorkerService> _logger;
	private readonly TimeSpan _interval;

	public EmailWorkerService(
		IServiceScopeFactory scopeFactory,
		IConfiguration config,
		ILogger<EmailWorkerService> logger)
	{
		_scopeFactory = scopeFactory;
		_config = config;
		_logger = logger;
		_interval = TimeSpan.FromSeconds(_config.GetValue<int>("EmailFetcher:IntervalSeconds", 300));
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		using var timer = new PeriodicTimer(_interval);

		do
		{
			try
			{
				await RunCycleAsync(cancellationToken);
			}
			catch (OperationCanceledException)
			{
				_logger.LogInformation("EmailWorkerService has been canceled");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Email processing error");
			}
		}
		while (await timer.WaitForNextTickAsync(cancellationToken));
	}

	private async Task RunCycleAsync(CancellationToken cancellationToken)
	{
		using var scope = _scopeFactory.CreateScope();
		var fetcher = scope.ServiceProvider.GetRequiredService<IEmailFetcherService>();
		var processor = scope.ServiceProvider.GetRequiredService<IEmailProcessorService>();

		await fetcher.FetchUnreadEmailsAsync(cancellationToken);
		await processor.ProcessNewEmailsAsync(cancellationToken);
	}
}
