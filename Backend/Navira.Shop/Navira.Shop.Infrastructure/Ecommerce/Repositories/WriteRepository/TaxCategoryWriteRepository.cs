using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Ecommerce;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Ecommerce
{
    public class TaxCategoryWriteRepository : WriteRepository<TaxCategory, int>, ITaxCategoryWriteRepository
    {
        public TaxCategoryWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
