

using Navira.Shop.Core.Entity;

namespace Navira.Shop.Core.Persistence.EF
{
    public interface IQueryRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
    }
}
