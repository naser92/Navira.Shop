using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class RolePolicyWriteRepository : WriteRepository<RolePolicy, int>, IRolePolicyWriteRepository
    {
        public RolePolicyWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }
    }
}
