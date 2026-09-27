using Navira.Shop.Core.Service;

namespace Navira.Shop.Application.Media
{
    public interface IMinIOFileService : IBaseService
    {
        Task<StoredImage> SaveAsync(Guid mediaFileId, PreparedImage image, CancellationToken cancellationToken = default);
    }
}
