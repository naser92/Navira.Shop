using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class BrandQueryRepository : QueryRepository<BrandModel, int>, IBrandQueryRepository
    {

        public BrandQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

