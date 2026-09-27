using Navira.Shop.Application.Media;
using Navira.Shop.Core.Caching;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class MediaFileQueryRepository : QueryRepository<MediaFileModel, Guid>, IMediaFileQueryRepository
    {

        public MediaFileQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }
}
