using Navira.Shop.Application.Media;
using Navira.Shop.Core.Caching;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class MediaVariantQueryRepository : QueryRepository<MediaVariantModel, long>, IMediaVariantQueryRepository
    {

        public MediaVariantQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }
}
