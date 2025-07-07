namespace Orders.Infrastructure.Utilities;

using System.Text.Json;

public static class OrderJsonValidator
{
	public static bool IsValid(string input)
	{
		try
		{
			using var doc = JsonDocument.Parse(input);
			var root = doc.RootElement;

			if (!root.TryGetProperty("id", out var id) || id.ValueKind != JsonValueKind.Number)
				return false;

			if (!root.TryGetProperty("products", out var products) || products.ValueKind != JsonValueKind.Array)
				return false;

			foreach (var product in products.EnumerateArray())
			{
				if (!product.TryGetProperty("productName", out var name) || name.ValueKind != JsonValueKind.String)
					return false;

				if (!product.TryGetProperty("quantity", out var qty) || qty.ValueKind != JsonValueKind.Number)
					return false;

				if (!product.TryGetProperty("price", out var price) || price.ValueKind != JsonValueKind.Number)
					return false;
			}

			return true;
		}
		catch
		{
			return false;
		}
	}
}