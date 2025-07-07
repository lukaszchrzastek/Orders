using Microsoft.AspNetCore.Components;
using Orders.Application.DTO;

namespace Orders.Presentation.Blazor
{
	public class OrderDataService(HttpClient httpClient, NavigationManager navigationManager)
	{
		public async Task<List<OrderDto>> GetOrdersAsync()
		{
			httpClient.BaseAddress ??= new Uri(navigationManager.BaseUri);
			var orders = await httpClient.GetFromJsonAsync<List<OrderDto>>("api/orders");
			return orders ?? [];
		}
	}
}