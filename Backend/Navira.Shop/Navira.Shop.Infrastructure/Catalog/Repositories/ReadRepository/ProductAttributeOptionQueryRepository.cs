using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductAttributeOptionQueryRepository : QueryRepository<ProductAttributeOptionModel, int>, IProductAttributeOptionQueryRepository
    {

        public ProductAttributeOptionQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

