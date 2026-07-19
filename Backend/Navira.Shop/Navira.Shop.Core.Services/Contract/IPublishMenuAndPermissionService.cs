using Navira.Shop.Core.Service;

namespace PDN.TPS.Framework.Services
{
    public interface IPublishMenuAndPermissionService : IBaseService
    {
        void BuildPermissionAndMenu(params string[] controllerNames);
    }
}
