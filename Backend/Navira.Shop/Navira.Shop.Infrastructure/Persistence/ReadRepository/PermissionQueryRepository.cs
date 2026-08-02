using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Caching;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class PermissionQueryRepository : QueryRepository<PermissionModel, int>, IPermissionQueryRepository
    {

        public PermissionQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }
}
