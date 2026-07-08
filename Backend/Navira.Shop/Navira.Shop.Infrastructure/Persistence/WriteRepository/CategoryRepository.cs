using Navira.Shop.Application.Catalog.Repository;
using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog.Entities;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class CategoryRepository : WriteRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }
    }
}
