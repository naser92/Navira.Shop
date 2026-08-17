using Navira.Shop.Core.Service;

namespace Navira.Shop.Application.Identity
{
    public interface IRoleQueryService : IBaseService
    {
        Task<IReadOnlyList<KeycloakRoleDto>> GetList();
    }
}
