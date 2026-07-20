using Navira.Shop.Core.Service;

namespace Navira.Shop.Core.Services
{
    public interface IPublishMenuAndPermissionService : IBaseService
    {
        Task BuildPermissionAndMenu(params string[] controllerNames);
    }
}
