using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class ProductImageWriteRepository : WriteRepository<ProductImage, long>, IProductImageWriteRepository
    {
        public ProductImageWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
