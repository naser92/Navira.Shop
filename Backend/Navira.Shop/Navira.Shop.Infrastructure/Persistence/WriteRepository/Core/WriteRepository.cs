using Microsoft.EntityFrameworkCore;
using Navira.Shop.Core.Caching;
using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Navira.Shop.Infrastructure.Persistence
{
    /// <summary>
    /// Represents the entity repository implementation
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class WriteRepository<TEntity, TKey> : IWriteRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        #region Fields

        private readonly IStaticCacheManager _staticCacheManager;
        private readonly WriteDbContext _context;

        private DbSet<TEntity> _entities;

        #endregion

        #region Ctor

        public WriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context)
        {
            _staticCacheManager = staticCacheManager;
            _context = context;
        }

        #endregion

        #region Methods

        public virtual async Task<TEntity> Get(TKey id)
        {
            return await Entities.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }


        public virtual async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            TEntity getEntity()
            {
                return Entities.FirstOrDefault(filter);
            }
            if (getCacheKey == null)
                return getEntity();

            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForShortTermCache(
                               new CacheKey($"{filter}"));
            return _staticCacheManager.Get(cacheKey, getEntity);
        }

        public virtual async Task<T> Get<T>(Expression<Func<TEntity, bool>> filter,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            async Task<T> getEntity()
            {
                var result = await Table.Where(filter).ProjectTo<T>().FirstOrDefaultAsync();
                return result;
            }
            if (getCacheKey == null)
                return await getEntity();

            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForShortTermCache(
                               new CacheKey($"{filter}->{typeof(T).Name}"));
            return await _staticCacheManager.Get(cacheKey, getEntity);
        }

        /// <summary>
        /// Get the entity entry
        /// </summary>
        /// <param name="id">Entity entry identifier</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entry</returns>
        public virtual async Task<TEntity> GetById(TKey id, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            if (id.Equals(null))
                return default;

            TEntity getEntity()
            {
                return Entities.FirstOrDefault(entity => entity.Id.Equals(id));
            }

            if (getCacheKey == null)
                return getEntity();

            //caching
            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyWithDefaultPrefixForDefaultCache(
                               EntityCacheDefaults<TEntity>.ByIdCacheKey, id);
            return _staticCacheManager.Get(cacheKey, getEntity);
        }
        //private async Task<TEntity> getEntity(TKey id)
        //{
        //    return await Entities.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        //}
        //private async Task<T> getEntity<T>(TKey id)
        //{
        //    var result = await Entities.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        //    return result.Map<T>();
        //}

        /// <summary>
        /// Get the entity entry
        /// </summary>
        /// <param name="id">Entity entry identifier</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entry</returns>
        public virtual async Task<T> GetById<T>(TKey id, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            if (id.Equals(null))
                return default;

            async Task<T> getEntityAsync()
            {
                return await Table.Where(entity => entity.Id.Equals(id)).ProjectTo<T>().FirstOrDefaultAsync();
            }

            if (getCacheKey == null)
                return await getEntityAsync();

            //caching
            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyWithDefaultPrefixForDefaultCache(
                               EntityCacheDefaults<TEntity>.ByIdCacheKey, id);
            return await _staticCacheManager.GetAsync(cacheKey, getEntityAsync);
        }

        /// <summary>
        /// Get entity entries by identifiers
        /// </summary>
        /// <param name="ids">Entity entry identifiers</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        public virtual async Task<IList<TEntity>> GetByIds(IList<TKey> ids,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            if (!ids?.Any() ?? true)
                return new List<TEntity>();

            IList<TEntity> getByIds()
            {
                var query = Table;

                //get entries
                var entries = query.Where(entry => ids.Contains(entry.Id)).ToList();

                //sort by passed identifiers
                var sortedEntries = new List<TEntity>();
                foreach (var id in ids)
                {
                    var sortedEntry = entries.FirstOrDefault(entry => entry.Id.Equals(id));
                    if (sortedEntry != null)
                        sortedEntries.Add(sortedEntry);
                }

                return sortedEntries;
            }

            if (getCacheKey == null)
                return getByIds();

            //caching
            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyWithDefaultPrefixForDefaultCache(
                               EntityCacheDefaults<TEntity>.ByIdsCacheKey, ids);
            return _staticCacheManager.Get(cacheKey, getByIds);
        }

        /// <summary>
        /// Get entity entries by identifiers
        /// </summary>
        /// <param name="ids">Entity entry identifiers</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        public virtual async Task<IList<T>> GetByIds<T>(IList<TKey> ids,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            if (!ids?.Any() ?? true)
                return new List<T>();

            async Task<IList<T>> getByIdsAsync()
            {
                var query = Table;

                //get entries
                var entries = await query.Where(entry => ids.Contains(entry.Id)).ProjectTo<T>().ToListAsync();

                //sort by passed identifiers
                //var sortedEntries = new List<TEntity>();
                //foreach (var id in ids)
                //{
                //    var sortedEntry = entries.FirstOrDefault(entry => entry.Id.Equals(id));
                //    if (sortedEntry != null)
                //        sortedEntries.Add(sortedEntry);
                //}

                return entries;
            }

            if (getCacheKey == null)
                return await getByIdsAsync();

            //caching
            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyWithDefaultPrefixForDefaultCache(
                               EntityCacheDefaults<TEntity>.ByIdsCacheKey, ids);
            return await _staticCacheManager.GetAsync(cacheKey, getByIdsAsync);
        }

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        public virtual async Task<IList<TEntity>> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            async Task<IList<TEntity>> getAllAsync()
            {
                var query = func != null ? func(Entities) : Entities;
                return await query.ToListAsync();
            }

            if (getCacheKey == null)
                return await getAllAsync();

            //caching
            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyWithDefaultPrefixForDefaultCache(EntityCacheDefaults<TEntity>
                               .AllCacheKey);
            return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
        }
        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="filter">filter to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        public virtual async Task<IList<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            async Task<IList<TEntity>> getAllAsync()
            {
                return await Table.Where(filter).ToListAsync();
            }

            if (getCacheKey == null)
                return await getAllAsync();

            //caching
            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyWithDefaultPrefixForDefaultCache(EntityCacheDefaults<TEntity>
                               .AllCacheKey);
            return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
        }
        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        public virtual async Task<IList<T>> GetAll<T>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            async Task<IList<T>> getAllAsync()
            {
                var query = func != null ? func(Table) : Table;
                return await query.ProjectTo<T>().ToListAsync();
            }

            if (getCacheKey == null)
                return await getAllAsync();

            //caching
            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyWithDefaultPrefixForDefaultCache(EntityCacheDefaults<TEntity>
                               .AllCacheKey);
            return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
        }
        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="filter">filter to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        public virtual async Task<IList<T>> GetAll<T>(Expression<Func<TEntity, bool>> filter, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            async Task<IList<T>> getAllAsync()
            {
                return await Table.Where(filter).ProjectTo<T>().ToListAsync();
            }

            if (getCacheKey == null)
                return await getAllAsync();

            //caching
            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyWithDefaultPrefixForDefaultCache(EntityCacheDefaults<TEntity>
                               .AllCacheKey);
            return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
        }

        /// <summary>
        /// Get paged list of all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">Whether to get only the total number of entries without actually loading data</param>
        /// <returns>Paged list of entity entries</returns>
        //public virtual IPagedList<TEntity> GetAllPaged(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
        //    int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        //{
        //    var query = func != null ? func(Table) : Table;

        //    return new PagedList<TEntity>(query, pageIndex, pageSize, getOnlyTotalCount);
        //}

        /// <summary>
        /// Insert the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        public virtual async Task Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await Entities.AddAsync(entity);

            //_dataProvider.InsertEntity(entity);

            //event notification
            //if (publishEvent)
            //_eventPublisher.EntityInserted(entity);
        }

        /// <summary>
        /// Insert entity entries
        /// </summary>
        /// <param name="entities">Entity entries</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        public virtual async Task Insert(IList<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            using var transaction = new TransactionScope();
            //_dataProvider.BulkInsertEntities(entities);
            Entities.AddRangeAsync(entities);
            transaction.Complete();

            // if (!publishEvent)
            //     return;

            //event notification
            //foreach (var entity in entities)
            //    _eventPublisher.EntityInserted(entity);
        }

        /// <summary>
        /// Update the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        public virtual async Task Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities.Update(entity);
        }

        /// <summary>
        /// Update entity entries
        /// </summary>
        /// <param name="entities">Entity entries</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        public virtual async Task Update(IList<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (!entities.Any())
                return;

            foreach (var entity in entities)
                Update(entity);
        }

        /// <summary>
        /// Delete the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        public virtual async Task Delete(TEntity entity)
        {
            Entities.Remove(entity);
        }

        /// <summary>
        /// Delete entity entries
        /// </summary>
        /// <param name="entities">Entity entries</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        public virtual async Task Delete(IList<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));


            Entities.RemoveRange(entities);
        }


        /// <summary>
        /// Delete entity entries by the passed predicate
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>Number of deleted records</returns>
        //public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        //{
        //    if (predicate == null)
        //        throw new ArgumentNullException(nameof(predicate));

        //    return _dataProvider.BulkDeleteEntities(predicate);
        //}

        /// <summary>
        /// Loads the original copy of the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <returns>Copy of the passed entity entry</returns>
        //public virtual TEntity LoadOriginalCopy(TEntity entity)
        //{
        //    return _dataProvider.GetTable<TEntity>()
        //        .FirstOrDefault(e => e.Id == Convert.ToInt32(entity.Id));
        //}

        /// <summary>
        /// Executes SQL using System.Data.CommandType.StoredProcedure command type and returns results as collection of values of specified type
        /// </summary>
        /// <param name="procedureName">Procedure name</param>
        /// <param name="parameters">Command parameters</param>
        /// <returns>Entity entries</returns>
        //public virtual IList<TEntity> EntityFromSql(string procedureName, params DataParameter[] parameters)
        //{
        //    return _dataProvider.QueryProc<TEntity>(procedureName, parameters?.ToArray());
        //}

        /// <summary>
        /// Truncates database table
        /// </summary>
        /// <param name="resetIdentity">Performs reset identity column</param>
        //public virtual void Truncate(bool resetIdentity = false)
        //{
        //    _dataProvider.GetTable<TEntity>().Truncate(resetIdentity);
        //}

        #endregion

        #region New_Methods
        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            async Task<bool> AnyAsync()
            {
                var result = await Table.AnyAsync(predicate);
                return result;
            }
            if (getCacheKey == null)
                return await AnyAsync();

            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForShortTermCache(
                               new CacheKey($"{predicate}->{typeof(bool).Name}"));
            return await _staticCacheManager.Get(cacheKey, AnyAsync);
        }

        public async Task<bool> All(Expression<Func<TEntity, bool>> predicate = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {

            async Task<bool> AllAsync()
            {
                var result = await Table.AllAsync(predicate);
                return result;
            }
            if (getCacheKey == null)
                return await AllAsync();

            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForShortTermCache(
                               new CacheKey($"{predicate}->{typeof(bool).Name}"));
            return await _staticCacheManager.Get(cacheKey, AllAsync);
        }

        public async Task<IEnumerable<TEntity>> List(Expression<Func<TEntity, bool>> predicate = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            async Task<IEnumerable<TEntity>> Where()
            {
                var result = await Table.Where(predicate).ToListAsync();
                return result;
            }
            if (getCacheKey == null)
                return await Where();

            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForShortTermCache(
                               new CacheKey($"{predicate}->{typeof(TEntity).Name}"));
            return await _staticCacheManager.Get(cacheKey, Where);
        }

        public async Task<IEnumerable<T>> List<T>(Expression<Func<TEntity, bool>> predicate = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {

            async Task<IEnumerable<T>> Where()
            {
                var result = await Table.Where(predicate).ProjectTo<T>().ToListAsync();
                return result;
            }
            if (getCacheKey == null)
                return await Where();

            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForShortTermCache(
                               new CacheKey($"{predicate}->{typeof(TEntity).Name}"));
            return await _staticCacheManager.Get(cacheKey, Where);
        }
        public async Task<IEnumerable<T>> List<T, TOrderKey>(Expression<Func<TEntity, bool>> predicate = null, Func<TEntity, TOrderKey> keySelector = null, string direction = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {

            async Task<IEnumerable<T>> Where()
            {
                var q = Table.Where(predicate);
                if (getCacheKey != null)
                    q = (direction?.ToLower() == "desc" ? q.OrderByDescending(keySelector) : q.OrderBy(keySelector)).AsQueryable();
                var result = await q.ProjectTo<T>().ToListAsync();
                return result;
            }
            if (getCacheKey == null)
                return await Where();

            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForShortTermCache(
                               new CacheKey($"{predicate}->{typeof(T).Name}"));
            return await _staticCacheManager.Get(cacheKey, Where);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => Entities.AsNoTracking();

        /// <summary>
        /// Gets an entity set
        /// </summary>
        public virtual DbSet<TEntity> Entities => _entities ??= _context.Set<TEntity>();

        #endregion
    }
}
