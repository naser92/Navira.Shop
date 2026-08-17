using Navira.Shop.Application.Auth;

namespace Navira.Shop.Application.Identity
{
    public class RoleQueryService : IRoleQueryService
    {
        private readonly IIdentityProviderClient _identityProviderClient;

        public RoleQueryService(IIdentityProviderClient identityProviderClient)
        {
            _identityProviderClient = identityProviderClient;
        }


        public async Task<IReadOnlyList<KeycloakRoleDto>> GetList()
        {
            return await _identityProviderClient.GetRealmRolesAsync();
        }
    }
}
