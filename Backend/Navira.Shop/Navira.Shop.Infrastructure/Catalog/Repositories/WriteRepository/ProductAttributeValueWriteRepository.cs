using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductAttributeValueWriteRepository : WriteRepository<ProductAttributeValue, long>, IProductAttributeValueWriteRepository
    {
        public ProductAttributeValueWriteRepository(IStaticCacheManager cache, WriteDbContext context) : base(cache, context) { }
    }
}
