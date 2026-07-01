using Navira.Shop.Core.Domain;

namespace Navira.Shop.Domain.Catalog.ValueObjects
{
    /// <summary>A stock-keeping-unit code, format-validated once at construction.</summary>
    public sealed class Sku : ValueObject
    {
        public string Value { get; }

        private Sku(string value) => Value = value;

        public static Sku Of(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("SKU cannot be empty.");

            var normalized = value.Trim().ToUpperInvariant();
            if (normalized.Length > 64)
                throw new DomainException("SKU cannot exceed 64 characters.");

            return new Sku(normalized);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }

}
