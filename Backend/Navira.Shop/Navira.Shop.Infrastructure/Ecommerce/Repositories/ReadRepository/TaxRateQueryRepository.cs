using Navira.Shop.Application.Ecommerce;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Ecommerce
{
    public class TaxRateQueryRepository : QueryRepository<TaxRateModel, int>, ITaxRateQueryRepository
    {

        public TaxRateQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

