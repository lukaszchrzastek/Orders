using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Interfaces;
using Orders.Application.Services;
using Orders.Domain.Repositories;
using Orders.Domain.Services;
using Orders.Infrastructure.Notifiers;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Persistence.Repositories;
using Orders.Infrastructure.Services;
using Orders.Infrastructure.Settings;

namespace Orders.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<ImapSettings>(configuration.GetSection(nameof(ImapSettings)));
		services.Configure<AzureOpenAISettings>(configuration.GetSection(nameof(AzureOpenAISettings)));

		services.AddScoped<IEmailProcessingRules, EmailProcessingRules>();
		services.AddScoped<IEmailFetcherService, EmailFetcherService>();
		services.AddScoped<IEmailProcessorService, EmailProcessorService>();
		services.AddScoped<IEmailClient, ImapEmailClient>();
		services.AddScoped<IEmailRepository, EmailRepository>();
		services.AddScoped<IOrderRepository, OrderRepository>();
		services.AddScoped<IOrderNotificationService, SignalROrderNotificationService>();

		services.AddSingleton<IOrderHtmlParserService, AzureOpenAIOrderHtmlService>();
		services.AddSingleton<IOrderUpdateNotifier, OrderUpdateNotifier>();
		services.AddHostedService<EmailWorkerService>();

		services.AddDbContext<AppDbContext>(options =>
			options.UseMySql(
				configuration.GetConnectionString("DefaultConnection"),
				ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))
			)
		);

		return services;
	}
}
