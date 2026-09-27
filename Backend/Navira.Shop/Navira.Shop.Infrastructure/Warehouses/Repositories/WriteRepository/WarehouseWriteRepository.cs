using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Warehouses;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Warehouses
{
    public class WarehouseWriteRepository : WriteRepository<Domain.Warehouses.Warehouse, int>, IWarehouseWriteRepository
    {
        public WarehouseWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
