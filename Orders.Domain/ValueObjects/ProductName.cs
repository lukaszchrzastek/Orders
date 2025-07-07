namespace Orders.Domain.ValueObjects
{
	public sealed class ProductName
	{
		public string Value { get; }

		private ProductName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Product name cannot be empty.", nameof(value));

			if (value.Length > 100)
				throw new ArgumentException("Product name cannot exceed 100 characters.", nameof(value));

			Value = value;
		}

		public static ProductName Create(string value) => new(value);				
	}
}
