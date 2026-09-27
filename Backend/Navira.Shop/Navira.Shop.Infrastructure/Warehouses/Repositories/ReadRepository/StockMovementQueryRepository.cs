using Navira.Shop.Application.Warehouses;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;


namespace Navira.Shop.Infrastructure.Warehouses
{
    public class StockMovementQueryRepository : QueryRepository<StockMovementModel, long>, IStockMovementQueryRepository
    {

        public StockMovementQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

