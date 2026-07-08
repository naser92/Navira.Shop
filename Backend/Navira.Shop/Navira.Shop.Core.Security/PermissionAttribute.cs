using Microsoft.AspNetCore.Mvc.Filters;

namespace Navira.Shop.Core.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : ActionFilterAttribute
    {
    }
}
