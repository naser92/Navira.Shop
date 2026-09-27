using Navira.Shop.Core.Caching;
using Navira.Shop.Domain.Media;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class MediaFileWriteRepository : WriteRepository<MediaFile, Guid>, IMediaFileWriteRepository
    {
        public MediaFileWriteRepository(IStaticCacheManager staticCacheManager, WriteDbContext context) : base(staticCacheManager, context)
        {
        }


    }
}
