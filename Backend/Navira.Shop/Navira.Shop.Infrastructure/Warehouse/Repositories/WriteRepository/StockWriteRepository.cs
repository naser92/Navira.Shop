using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Warehouse;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class StockWriteRepository : WriteRepository<Stock, long>, IStockWriteRepository
    {
        public StockWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
