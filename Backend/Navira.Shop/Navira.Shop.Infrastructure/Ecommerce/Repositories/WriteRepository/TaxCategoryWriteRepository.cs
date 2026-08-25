using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Ecommerce;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class TaxCategoryWriteRepository : WriteRepository<TaxCategory, int>, ITaxCategoryWriteRepository
    {
        public TaxCategoryWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
