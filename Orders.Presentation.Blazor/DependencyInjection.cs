namespace Orders.Presentation.Blazor;

public static class DependencyInjection
{
	public static IServiceCollection AddPresentation(this IServiceCollection services)
	{
		services.AddHttpClient<OrderDataService>();
		return services;
	}
}