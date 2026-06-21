using Navira.Shop.Core.Extensions;

namespace Navira.Shop.Core.Caching
{
    /// <summary>
    /// Represents default values related to caching entities
    /// </summary>
    public static partial class CacheKeyGenerator
    {
        public static CacheKey GeneratCacheKey(params string[] keys)
        {
            var validkeys = keys.Where(x => x.IsNotNullOrEmpty()).ToArray();
            return new CacheKey(validkeys.Join(":"), validkeys.ToCacheKey());
        }
    }
}