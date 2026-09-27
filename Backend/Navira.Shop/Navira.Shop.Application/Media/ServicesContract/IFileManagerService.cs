using Navira.Shop.Core.Results;
using Navira.Shop.Core.Service;

namespace Navira.Shop.Application.Media
{
    public interface IFileManagerService : IBaseService
    {
        Task<IResult> SaveImage(UploadMediaRequest request, CancellationToken cancellationToken = default);
    }
}
