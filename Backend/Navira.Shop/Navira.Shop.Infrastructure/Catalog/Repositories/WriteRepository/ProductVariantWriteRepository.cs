using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class ProductVariantWriteRepository : WriteRepository<ProductVariant, int>, IProductVariantWriteRepository
    {
        public ProductVariantWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
