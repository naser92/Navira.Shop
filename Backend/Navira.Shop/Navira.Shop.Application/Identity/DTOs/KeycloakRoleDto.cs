namespace Navira.Shop.Application.Identity
{
    public class KeycloakRoleDto
    {
        public string Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Description { get; set; }

        public bool Composite { get; set; }

        public bool ClientRole { get; set; }

        public string ContainerId { get; set; }
    }
}
