using Navira.Shop.Application.Catalog.Repository;
using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog.Entities;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class CategoriesRepository : WriteRepository<Categories, int>, ICategoriesRepository
    {
        public CategoriesRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }
    }
}
