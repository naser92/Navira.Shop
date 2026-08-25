using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductAttributeWriteRepository : WriteRepository<ProductAttribute, int>, IProductAttributeWriteRepository
    {
        public ProductAttributeWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
