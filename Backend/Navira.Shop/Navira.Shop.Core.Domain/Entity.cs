using Navira.Shop.Core.Entity;

namespace Navira.Shop.Core.Domain
{
    public abstract class Entity<TId> : IEntity<TId>, IEquatable<Entity<TId>>
    {
        public TId Id { get; set; } = default!;
        protected Entity() { }

        protected Entity(TId id)
        {
            Id = id;
        }

        public override bool Equals(object? obj) => Equals(obj as Entity<TId>);

        public bool Equals(Entity<TId>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return Equals(Id, other.Id);
        }

        public override int GetHashCode() => HashCode.Combine(GetType(), Id);

        public static bool operator ==(Entity<TId>? a, Entity<TId>? b) =>
            a is null ? b is null : a.Equals(b);

        public static bool operator !=(Entity<TId>? a, Entity<TId>? b) => !(a == b);


    }

    public class FullEntity<TKey> : Entity<TKey>, ISoftDeletableEntity, IHasRowVersion
    {

        public FullEntity()
        {
        }

        public FullEntity(TKey id)
        {
            Id = id;
        }
        public bool IsDeleted { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
