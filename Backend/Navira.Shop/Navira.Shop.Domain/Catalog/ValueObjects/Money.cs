using Navira.Shop.Core.Domain;

namespace Navira.Shop.Domain.Catalog.ValueObjects
{
    public sealed class Money : ValueObject
    {
        public decimal Amount { get; }
        public string CurrencyCode { get; }

        private Money(decimal amount, string currencyCode)
        {
            Amount = amount;
            CurrencyCode = currencyCode;
        }

        public static Money Of(decimal amount, string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
                throw new DomainException($"Currency code must be a 3-letter ISO code, got '{currencyCode}'.");
            if (amount < 0)
                throw new DomainException("Money amount cannot be negative.");

            return new Money(decimal.Round(amount, 2, MidpointRounding.AwayFromZero), currencyCode.ToUpperInvariant());
        }

        public static Money Zero(string currencyCode) => Of(0m, currencyCode);

        public Money Add(Money other)
        {
            EnsureSameCurrency(other);
            return Of(Amount + other.Amount, CurrencyCode);
        }

        public Money Subtract(Money other)
        {
            EnsureSameCurrency(other);
            var result = Amount - other.Amount;
            if (result < 0)
                throw new DomainException($"Subtracting {other} from {this} would produce a negative amount.");
            return Of(result, CurrencyCode);
        }

        public Money MultiplyBy(int quantity)
        {
            if (quantity < 0)
                throw new DomainException("Cannot multiply a monetary amount by a negative quantity.");
            return Of(Amount * quantity, CurrencyCode);
        }

        public Money ApplyPercentage(decimal percentage)
        {
            if (percentage < 0)
                throw new DomainException("Percentage cannot be negative.");
            return Of(Amount * percentage / 100m, CurrencyCode);
        }

        private void EnsureSameCurrency(Money other)
        {
            if (CurrencyCode != other.CurrencyCode)
                throw new DomainException($"Cannot operate on Money with different currencies: {CurrencyCode} vs {other.CurrencyCode}.");
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return CurrencyCode;
        }

        public override string ToString() => $"{Amount:0.00} {CurrencyCode}";




    }
}
