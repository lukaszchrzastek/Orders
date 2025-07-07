namespace Orders.Domain.ValueObjects
{
	public sealed class OrderId
	{
		public int Value { get; }

		private OrderId(int value)
		{
			if (value <= 0)
				throw new ArgumentOutOfRangeException(nameof(value), "Order ID must be greater than zero.");
			Value = value;
		}

		public static OrderId Create(int value) => new(value);
	}
}
