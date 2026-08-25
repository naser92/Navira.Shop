using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class CategoryQueryRepository : QueryRepository<CategoryModel, int>, ICategoryQueryRepository
    {

        public CategoryQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

