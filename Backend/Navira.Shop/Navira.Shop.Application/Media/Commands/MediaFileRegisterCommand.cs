using Navira.Shop.Core.Bus;

namespace Navira.Shop.Application.Media
{
    public class MediaFileRegisterCommand : ICommand
    {
        public MediaFileRegisterCommand(UploadMediaRequest data)
        {
            RequestData = data;
        }
        public UploadMediaRequest RequestData { get; set; }
    }
}
