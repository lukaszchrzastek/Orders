namespace Orders.Domain.ValueObjects
{
	public sealed class Quantity
	{
		public int Value { get; }

		private Quantity(int value)
		{
			if (value <= 0)
				throw new ArgumentOutOfRangeException(nameof(value), "Quantity must be greater than zero.");
			Value = value;
		}

		public static Quantity Create(int value) => new(value);
	}
}
