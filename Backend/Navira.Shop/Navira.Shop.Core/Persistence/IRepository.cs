using System.Linq.Expressions;

namespace Navira.Shop.Core.Persistence
{
    public interface IRepository<T, TKey>
    {
        Task<T> Get(TKey id);
        Task<T> FindBy(Expression<Func<T, bool>> predicate);
        Task Delete(T entity);
        Task Insert(T aggregate);
        Task Update(T aggregate);

    }
}
