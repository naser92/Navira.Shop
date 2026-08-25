using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductVariantAttributeValueQueryRepository : QueryRepository<ProductVariantAttributeValueModel, long>, IProductVariantAttributeValueQueryRepository
    {

        public ProductVariantAttributeValueQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

