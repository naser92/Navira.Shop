using Navira.Shop.Core.Entity;

namespace Navira.Shop.Core.Persistence.EF
{
    public interface IWriteRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
    }
}
