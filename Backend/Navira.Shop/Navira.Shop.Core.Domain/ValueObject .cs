namespace Navira.Shop.Core.Domain
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        protected abstract IEnumerable<object?> GetEqualityComponents();

        public override bool Equals(object? obj) => Equals(obj as ValueObject);

        public bool Equals(ValueObject? other)
        {
            if (other is null || other.GetType() != GetType()) return false;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode() =>
            GetEqualityComponents()
                .Select(c => c?.GetHashCode() ?? 0)
                .Aggregate(17, (a, b) => a * 31 + b);

        public static bool operator ==(ValueObject? a, ValueObject? b) =>
            a is null ? b is null : a.Equals(b);

        public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);
    }

}
