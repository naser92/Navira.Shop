using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Warehouse;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Warehouse
{
    public class StockMovementWriteRepository : WriteRepository<StockMovement, long>, IStockMovementWriteRepository
    {
        public StockMovementWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
