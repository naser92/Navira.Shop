using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Warehouses;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Warehouses
{
    public class StockWriteRepository : WriteRepository<Stock, long>, IStockWriteRepository
    {
        public StockWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
