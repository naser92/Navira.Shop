using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Ecommerce;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class TaxRateWriteRepository : WriteRepository<TaxRate, int>, ITaxRateWriteRepository
    {
        public TaxRateWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
