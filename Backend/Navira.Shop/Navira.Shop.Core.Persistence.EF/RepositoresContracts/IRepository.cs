using Navira.Shop.Core.Caching;
using Navira.Shop.Core.Entity;
using System.Linq.Expressions;

namespace Navira.Shop.Core.Persistence.EF
{
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        #region Methods

        Task<TEntity> Get(TKey id);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);
        Task<T> Get<T>(Expression<Func<TEntity, bool>> predicate = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        Task<bool> Any(Expression<Func<TEntity, bool>> predicate = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);
        Task<bool> All(Expression<Func<TEntity, bool>> predicate = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        Task<IEnumerable<TEntity>> List(Expression<Func<TEntity, bool>> predicate = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);
        Task<IEnumerable<T>> List<T>(Expression<Func<TEntity, bool>> predicate = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);
        Task<IEnumerable<T>> List<T, TOrderKey>(Expression<Func<TEntity, bool>> predicate = null, Func<TEntity, TOrderKey> keyselector = null, string direction = "", Func<IStaticCacheManager, CacheKey> getCacheKey = null);
        /// <summary>
        /// Get the entity entry
        /// </summary>
        /// <param name="id">Entity entry identifier</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entry</returns>
        /// 
        Task<TEntity> GetById(TKey id, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        /// <summary>
        /// Get the entity entry
        /// </summary>
        /// <param name="id">Entity entry identifier</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entry</returns>
        /// 
        Task<T> GetById<T>(TKey id, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        /// <summary>
        /// Get entity entries by identifiers
        /// </summary>
        /// <param name="ids">Entity entry identifiers</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        Task<IList<TEntity>> GetByIds(IList<TKey> ids, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        /// <summary>
        /// Get entity entries by identifiers
        /// </summary>
        /// <param name="ids">Entity entry identifiers</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        Task<IList<T>> GetByIds<T>(IList<TKey> ids, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        Task<IList<TEntity>> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        Task<IList<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        Task<IList<T>> GetAll<T>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        Task<IList<T>> GetAll<T>(Expression<Func<TEntity, bool>> filter, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        /// <summary>
        /// Get paged list of all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">Whether to get only the total number of entries without actually loading data</param>
        /// <returns>Paged list of entity entries</returns>
        //IPagedList<TEntity> GetAllPaged(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
        //    int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        /// <summary>
        /// Insert the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        Task Insert(TEntity entity);

        /// <summary>
        /// Insert entity entries
        /// </summary>
        /// <param name="entities">Entity entries</param>
        Task Insert(IList<TEntity> entities);

        /// <summary>
        /// Update the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        Task Update(TEntity entity);

        /// <summary>
        /// Update entity entries
        /// </summary>
        /// <param name="entities">Entity entries</param>
        Task Update(IList<TEntity> entities);

        /// <summary>
        /// Delete the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        Task Delete(TEntity entity);

        /// <summary>
        /// Delete entity entries
        /// </summary>
        /// <param name="entities">Entity entries</param>
        Task Delete(IList<TEntity> entities);

        /// <summary>
        /// Delete entity entries by the passed predicate
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>Number of deleted records</returns>
       // int Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Loads the original copy of the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <returns>Copy of the passed entity entry</returns>
       // TEntity LoadOriginalCopy(TEntity entity);

        /// <summary>
        /// Executes SQL using System.Data.CommandType.StoredProcedure command type and returns results as collection of values of specified type
        /// </summary>
        /// <param name="procedureName">Procedure name</param>
        /// <param name="parameters">Command parameters</param>
        /// <returns>Entity entries</returns>
        //IList<TEntity> EntityFromSql(string procedureName, params DataParameter[] parameters);

        /// <summary>
        /// Truncates database table
        /// </summary>
        /// <param name="resetIdentity">Performs reset identity column</param>
      //  void Truncate(bool resetIdentity = false);

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }

        #endregion
    }
}