using Navira.Shop.Core.Service;

namespace Navira.Shop.Application.Media
{
    public interface IMediaPersistenceService : IBaseService
    {
        Task CreateUploadingAsync(
        Guid mediaFileId,
        string originalFileName,
        PreparedImageMetadata metadata,
        CancellationToken cancellationToken);

        Task CompleteAsync(
            Guid mediaFileId,
            StoredImage image,
            CancellationToken cancellationToken);
    }
}
