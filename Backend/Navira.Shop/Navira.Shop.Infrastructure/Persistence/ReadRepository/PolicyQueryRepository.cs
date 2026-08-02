using Navira.Shop.Application.Identity;
using Navira.Shop.Application.Identity.Repository;
using Navira.Shop.Core.Caching;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class PolicyQueryRepository : QueryRepository<PolicyModel, int>, IPolicyQueryRepository
    {

        public PolicyQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }
}
