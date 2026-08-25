using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductImageQueryRepository : QueryRepository<ProductImageModel, long>, IProductImageQueryRepository
    {

        public ProductImageQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

