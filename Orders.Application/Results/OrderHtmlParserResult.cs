namespace Orders.Application.Results
{
	public class OrderHtmlParserResult
	{
		public bool Success { get; init; }
		public string? ErrorMessage { get; init; }
		public Domain.Models.Order? Order { get; init; }
	}
}
