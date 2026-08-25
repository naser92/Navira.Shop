using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductVariantWriteRepository : WriteRepository<ProductVariant, int>, IProductVariantWriteRepository
    {
        public ProductVariantWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
