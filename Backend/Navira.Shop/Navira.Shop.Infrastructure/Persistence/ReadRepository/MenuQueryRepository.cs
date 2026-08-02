using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Caching;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class MenuQueryRepository : QueryRepository<MenuModel, int>, IMenuQueryRepository
    {

        public MenuQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }
}
