using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Warehouse;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class WarehouseWriteRepository : WriteRepository<Warehouse, int>, IWarehouseWriteRepository
    {
        public WarehouseWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
