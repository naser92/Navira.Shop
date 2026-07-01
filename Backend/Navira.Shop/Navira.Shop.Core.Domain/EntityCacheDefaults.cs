using Navira.Shop.Core.Caching;

namespace Navira.Shop.Core.Domain
{
    /// <summary>
    /// Represents default values related to caching entities
    /// </summary>
    public static partial class EntityCacheDefaults<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets an entity type name used in cache keys
        /// </summary>
        public static string EntityTypeName => typeof(TEntity).Name;

        /// <summary>
        /// Gets a key for caching entity by identifier
        /// </summary>
        /// <remarks>
        /// {0} : entity id
        /// </remarks>
        public static CacheKey ByIdCacheKey => new CacheKey(ByIdPrefix, Prefix);

        /// <summary>
        /// Gets a key for caching entities by identifiers
        /// </summary>
        /// <remarks>
        /// {0} : entity ids
        /// </remarks>
        public static CacheKey ByIdsCacheKey => new CacheKey(ByIdsPrefix, Prefix);

        /// <summary>
        /// Gets a key for caching all entities
        /// </summary>
        public static CacheKey AllCacheKey => new CacheKey(AllPrefix, Prefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string Prefix => $"{EntityTypeName}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ByIdPrefix => $"{Prefix}:byid:{{0}}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ByIdsPrefix => $"{Prefix}:byids:{{0}}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllPrefix => $"{Prefix}:all";
    }
}