using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Media;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class MediaVariantWriteRepository : WriteRepository<MediaVariant, long>, IMediaVariantWriteRepository
    {
        public MediaVariantWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
