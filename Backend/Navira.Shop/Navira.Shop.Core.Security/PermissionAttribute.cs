using Microsoft.AspNetCore.Mvc.Filters;

namespace Navira.Shop.Core.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : ActionFilterAttribute
    {
        public string Resourc { get; set; }
        public string Scop { get; set; }

        private string Permission { get; set; }
    }
}
