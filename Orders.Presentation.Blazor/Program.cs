using Microsoft.EntityFrameworkCore;
using Orders.Application;
using Orders.Infrastructure;
using Orders.Infrastructure.Hubs;
using Orders.Infrastructure.Persistence;

namespace Orders.Presentation.Blazor;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		
		builder.Services.AddControllers();
		builder.Services.AddRazorPages();
		builder.Services.AddServerSideBlazor();
		builder.Services.AddSignalR();
		builder.Services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy.WithOrigins("http://localhost:8080")
					  .AllowAnyMethod()
					  .AllowAnyHeader()
					  .AllowCredentials();
			});
		});
		builder.Services.AddSingleton(TimeProvider.System);
		//builder.Logging.AddConsole();
		
		builder.Services.AddPresentation();
		builder.Services.AddInfrastructure(builder.Configuration);
		builder.Services.AddApplication();

		var app = builder.Build();
		
		using (var scope = app.Services.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			dbContext.Database.Migrate();
		}
		
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error");
		}

		app.UseStaticFiles();
		app.UseCors();
		app.UseRouting();
		app.UseAntiforgery();
		
		app.MapGet("/api/ping", () => Results.Ok("pong"));
		app.MapControllers();
		app.MapHub<OrderHub>("/orderhub");

		app.MapRazorPages();
		app.MapBlazorHub();
		app.MapFallbackToPage("/_Host");

		app.Logger.LogInformation("Blazor Server app started at {Time}", DateTimeOffset.Now);

		app.Run();
	}
}