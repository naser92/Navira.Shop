using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class PolicyPermissionWriteRepository : WriteRepository<PolicyPermission, int>, IPolicyPermissionWriteRepository
    {
        public PolicyPermissionWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
