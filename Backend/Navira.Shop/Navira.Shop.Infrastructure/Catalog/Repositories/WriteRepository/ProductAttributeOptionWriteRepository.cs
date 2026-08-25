using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class ProductAttributeOptionWriteRepository : WriteRepository<ProductAttributeOption, int>, IProductAttributeOptionWriteRepository
    {
        public ProductAttributeOptionWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
