using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class PolicyWriteRepository : WriteRepository<Policy, int>, IPolicyWriteRepository
    {
        public PolicyWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
