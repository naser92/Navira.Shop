using Navira.Shop.Core.Service;
using Navira.Shop.Domain.Media;

namespace Navira.Shop.Application.Media
{
    public interface IImagePreparationService : IBaseService
    {
        Task<PreparedImage> PrepareAsync(Stream source, MediaUploadProfile profile, ImageEditDto edit, CancellationToken cancellationToken);
    }
}
