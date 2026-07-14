using Microsoft.AspNetCore.Authorization;

namespace Navira.Shop.Core.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAttribute : AuthorizeAttribute
    {

        public PermissionAttribute(string method, string controller, string app)
        {
            Policy = $"{method}.{controller}.{app}";
        }

        public PermissionAttribute(string permission)
        {
            Policy = permission;
        }

        public string Permission
        {
            get => Policy?.StartsWith($"{PermissionRequirement.PolicyPrefix}:", StringComparison.Ordinal) == true
                ? Policy.Substring($"{PermissionRequirement.PolicyPrefix}:".Length)
                : string.Empty;
            set => Policy = $"{PermissionRequirement.PolicyPrefix}:{value}";
        }
    }
}

