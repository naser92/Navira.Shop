
namespace Navira.Shop.Application.Media
{
    public class MediaStorageService : IMediaStorageService
    {
        private readonly IMinIOFileService _minioFileService;

        public MediaStorageService(IMinIOFileService minioFileService)
        {
            _minioFileService = minioFileService;
        }

        public async Task<StoredImage> SaveAsync(Guid mediaFileId, PreparedImage image, CancellationToken cancellationToken)
        {
            return await _minioFileService.SaveAsync(mediaFileId, image, cancellationToken);
        }
    }
}
