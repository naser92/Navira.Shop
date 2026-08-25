using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Warehouse;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class StockMovementWriteRepository : WriteRepository<StockMovement, long>, IStockMovementWriteRepository
    {
        public StockMovementWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
