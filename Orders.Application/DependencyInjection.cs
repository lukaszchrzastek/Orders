using Microsoft.Extensions.DependencyInjection;
using Orders.Domain.Services;

namespace Orders.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IEmailService, EmailService>();
		services.AddScoped<IOrderService, OrderService>();
		services.AddScoped<IEmailUniquenessChecker, EmailUniquenessChecker>();

		return services;
	}
}