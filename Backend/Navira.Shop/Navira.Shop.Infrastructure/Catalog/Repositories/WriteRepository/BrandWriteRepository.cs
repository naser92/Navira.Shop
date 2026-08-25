using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Catalog;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class BrandWriteRepository : WriteRepository<Brand, int>, IBrandWriteRepository
    {
        public BrandWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
