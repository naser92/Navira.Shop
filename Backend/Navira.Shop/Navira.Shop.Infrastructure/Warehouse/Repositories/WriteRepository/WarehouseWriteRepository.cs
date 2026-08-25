using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Warehouse;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Warehouse
{
    public class WarehouseWriteRepository : WriteRepository<Domain.Warehouse.Warehouse, int>, IWarehouseWriteRepository
    {
        public WarehouseWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
