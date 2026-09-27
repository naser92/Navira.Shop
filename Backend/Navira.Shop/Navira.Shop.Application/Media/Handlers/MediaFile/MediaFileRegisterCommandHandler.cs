using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Media
{
    public class MediaFileRegisterCommandHandler : CommandHandler, ICommandHandler<MediaFileRegisterCommand>
    {
        private readonly IFileManagerService _fileManagerService;
        public MediaFileRegisterCommandHandler(IUnitOfWork uow, IFileManagerService fileManagerService) : base(uow)
        {
            _fileManagerService = fileManagerService;
        }

        public async Task<IResult> Handle(MediaFileRegisterCommand command, CancellationToken cancellationToken = default)
        {
            return await _fileManagerService.SaveImage(command.RequestData, cancellationToken);
        }
    }
}
