using Navira.Shop.Application.Identity;
using Navira.Shop.Application.Identity.Repository;
using Navira.Shop.Core.Caching;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class RolePolicyQueryRepository : QueryRepository<RolePolicyModel, int>, IRolePolicyQueryRepository
    {

        public RolePolicyQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }
}
