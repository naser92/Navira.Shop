using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductVariantAttributeValueWriteRepository : WriteRepository<ProductVariantAttributeValue, long>, IProductVariantAttributeValueWriteRepository
    {
        public ProductVariantAttributeValueWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
