using Navira.Shop.Core.Service;

namespace Navira.Shop.Application.Media
{
    public interface IMediaStorageService : IBaseService
    {
        Task<StoredImage> SaveAsync(
        Guid mediaFileId,
        PreparedImage image,
        CancellationToken cancellationToken);
    }
}
