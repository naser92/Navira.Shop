using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class MenuWriteRepository : WriteRepository<Menu, int>, IMenuWriteRepository
    {
        public MenuWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
