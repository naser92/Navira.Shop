using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Ecommerce;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Ecommerce
{
    public class TaxRateWriteRepository : WriteRepository<TaxRate, int>, ITaxRateWriteRepository
    {
        public TaxRateWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
