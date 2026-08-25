namespace Navira.Shop.Infrastructure.Persistence
{
    public class BrandQueryRepository : QueryRepository<BrandModel, int>, IBrandQueryRepository
    {

        public BrandQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

