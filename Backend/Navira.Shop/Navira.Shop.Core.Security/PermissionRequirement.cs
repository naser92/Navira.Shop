using Microsoft.AspNetCore.Authorization;

namespace Navira.Shop.Core.Security
{
    public sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public const string PolicyPrefix = "Permission";
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

        public string Permission { get; }

        public override string ToString()
            => Permission;
    }
}
