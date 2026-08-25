using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductAttributeOptionWriteRepository : WriteRepository<ProductAttributeOption, int>, IProductAttributeOptionWriteRepository
    {
        public ProductAttributeOptionWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
