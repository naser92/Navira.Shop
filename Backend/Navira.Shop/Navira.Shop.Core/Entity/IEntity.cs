namespace Navira.Shop.Core.Entity
{
    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
    public interface ISoftDeletableEntity
    {
        public bool IsDeleted { get; set; }
    }
    public interface IHasRowVersion
    {
        public byte[] RowVersion { get; set; }
    }

    public interface IMemoryOptimize
    {
    }
}