using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Caching;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class PolicyPermissionQueryRepository : QueryRepository<PolicyPermissionModel, int>, IPolicyPermissionQueryRepository
    {

        public PolicyPermissionQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }
}
