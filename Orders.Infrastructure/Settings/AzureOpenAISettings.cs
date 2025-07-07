namespace Orders.Infrastructure.Settings
{
	public class AzureOpenAISettings
	{
		public string Endpoint { get; init; }
		public string ApiKey { get; init; }
		public string Model { get; init; }
		public string Deployment { get; init; }
	}
}