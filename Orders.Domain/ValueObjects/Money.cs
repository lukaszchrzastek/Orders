namespace Orders.Domain.ValueObjects
{
	public sealed class Money
	{
		public decimal Amount { get; }
		public string Currency { get; }

		private Money(decimal amount, string currency)
		{
			if (amount < 0)
				throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");
			if (string.IsNullOrWhiteSpace(currency))
				throw new ArgumentException("Currency is required.", nameof(currency));

			Amount = amount;
			Currency = currency.ToUpperInvariant();
		}

		public static Money Create(decimal amount, string currency) => new(amount, currency);		
	}
}
