using Microsoft.AspNetCore.Authorization;

namespace Navira.Shop.Core.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAttribute : AuthorizeAttribute
    {

        public PermissionAttribute()
        {
        }

        public PermissionAttribute(string scope)
        {
            Scope = scope;
        }

        public string? Scope { get; }
    }
}

