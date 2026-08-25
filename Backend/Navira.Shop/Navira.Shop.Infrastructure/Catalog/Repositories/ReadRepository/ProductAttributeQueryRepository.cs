using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductAttributeQueryRepository : QueryRepository<ProductAttributeModel, int>, IProductAttributeQueryRepository
    {

        public ProductAttributeQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

