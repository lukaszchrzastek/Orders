using System.Text.Json;

namespace Orders.Infrastructure.Serialization
{
	public static class JsonDefaults
	{
		public static readonly JsonSerializerOptions CaseInsensitive = new()
		{
			PropertyNameCaseInsensitive = true
		};
	}
}
