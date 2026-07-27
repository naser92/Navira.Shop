using Navira.Shop.Core.Caching;

namespace Navira.Shop.Core.Security
{

    public class RedisPermissionProvider : IPermissionProvider
    {
        private readonly IStaticCacheManager _staticCacheManager;
        public RedisPermissionProvider(IStaticCacheManager staticCacheManager)
        {
            _staticCacheManager = staticCacheManager;
        }

        public async Task<HashSet<string>> GetPermissionsAsync(string role)
        {
            var tree = _staticCacheManager.Get<PermissionTree>(new CacheKey($"PermissionTree:{role}"));


            return tree.Policies
                .SelectMany(x => x.Permissions.Select(i => i.Code))
                .ToHashSet();
        }
    }
}
