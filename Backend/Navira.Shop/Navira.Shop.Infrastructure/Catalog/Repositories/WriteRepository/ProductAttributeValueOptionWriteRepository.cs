using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductAttributeValueOptionWriteRepository : WriteRepository<ProductAttributeValueOption, long>, IProductAttributeValueOptionWriteRepository
    {
        public ProductAttributeValueOptionWriteRepository(IStaticCacheManager cache, WriteDbContext context) : base(cache, context) { }
    }
}
