using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class PermissionWriteRepository : WriteRepository<Permission, int>, IPermissionWriteRepository
    {
        public PermissionWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }

    }
}
