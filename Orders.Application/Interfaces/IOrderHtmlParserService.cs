using Orders.Application.Results;

namespace Orders.Application.Interfaces
{
	public interface IOrderHtmlParserService
	{
		Task<OrderHtmlParserResult> ParseAsync(
			string html, 			
			CancellationToken cancellationToken = default);
	}
}