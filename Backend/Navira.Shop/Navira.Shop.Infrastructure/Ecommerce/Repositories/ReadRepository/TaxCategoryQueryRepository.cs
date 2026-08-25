using Navira.Shop.Application.Ecommerce;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Ecommerce
{
    public class TaxCategoryQueryRepository : QueryRepository<TaxCategoryModel, int>, ITaxCategoryQueryRepository
    {

        public TaxCategoryQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

