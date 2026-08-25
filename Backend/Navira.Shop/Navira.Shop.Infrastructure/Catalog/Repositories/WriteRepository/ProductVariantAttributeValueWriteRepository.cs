using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class ProductVariantAttributeValueWriteRepository : WriteRepository<ProductVariantAttributeValue, long>, IProductVariantAttributeValueWriteRepository
    {
        public ProductVariantAttributeValueWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
