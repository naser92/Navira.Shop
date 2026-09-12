namespace Navira.Shop.Application.Identity
{
    public class RoleListDto
    {
        public IReadOnlyList<KeycloakRoleDto> Data { get; set; }
        public int TotalCount { get; set; }
    }
}
