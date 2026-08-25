using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class ProductAttributeWriteRepository : WriteRepository<ProductAttribute, int>, IProductAttributeWriteRepository
    {
        public ProductAttributeWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
