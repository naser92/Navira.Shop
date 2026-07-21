using Navira.Shop.Core.Service;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Core.Services
{
    public interface IPublishMenuAndPermissionService : IBaseService
    {
        Task<RegisterMenuAndPermissionEvent> BuildPermissionAndMenu(params string[] controllerNames);
    }
}
