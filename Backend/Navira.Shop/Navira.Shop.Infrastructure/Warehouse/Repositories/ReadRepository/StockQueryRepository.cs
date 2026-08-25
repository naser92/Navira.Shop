using Navira.Shop.Application.Warehouse;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;


namespace Navira.Shop.Infrastructure.Warehouse
{
    public class StockQueryRepository : QueryRepository<StockModel, long>, IStockQueryRepository
    {

        public StockQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

