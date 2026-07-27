namespace Navira.Shop.Core.Security
{
    public sealed class PermissionTree
    {
        public required string Role { get; init; }

        public required IReadOnlyCollection<PolicyNode> Policies { get; init; }

        public long Version { get; init; }

        public DateTime LastUpdatedUtc { get; init; }
    }

    public sealed class PolicyNode
    {
        public required string Name { get; init; }

        public required IReadOnlyCollection<PermissionNode> Permissions { get; init; }
    }

    public sealed class PermissionNode
    {
        public required string Code { get; init; }

        public required string Title { get; init; }
    }
}
