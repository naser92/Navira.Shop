using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductImageWriteRepository : WriteRepository<ProductImage, long>, IProductImageWriteRepository
    {
        public ProductImageWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
