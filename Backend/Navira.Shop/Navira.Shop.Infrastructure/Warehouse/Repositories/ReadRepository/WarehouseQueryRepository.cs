using Navira.Shop.Application.Warehouse;
using Navira.Shop.Core.Caching;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Infrastructure.Warehouse
{
    public class WarehouseQueryRepository : QueryRepository<WarehouseModel, int>, IWarehouseQueryRepository
    {

        public WarehouseQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

