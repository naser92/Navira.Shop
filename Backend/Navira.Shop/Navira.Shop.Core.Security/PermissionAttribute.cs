using Microsoft.AspNetCore.Authorization;

namespace Navira.Shop.Core.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAttribute : AuthorizeAttribute
    {

        public PermissionAttribute(string title)
        {
            Title = title;
        }

        public PermissionAttribute(string scope, string title)
        {
            Scope = scope;
            Title = title;
        }



        public string? Scope { get; }

        public string? Title { get; set; }



    }
}

