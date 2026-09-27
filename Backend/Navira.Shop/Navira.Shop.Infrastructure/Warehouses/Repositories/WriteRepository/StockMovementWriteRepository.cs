using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Warehouses;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Warehouses
{
    public class StockMovementWriteRepository : WriteRepository<StockMovement, long>, IStockMovementWriteRepository
    {
        public StockMovementWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
