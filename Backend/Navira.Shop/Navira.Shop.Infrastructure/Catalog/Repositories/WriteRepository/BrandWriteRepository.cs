using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class BrandWriteRepository : WriteRepository<Brand, int>, IBrandWriteRepository
    {
        public BrandWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
