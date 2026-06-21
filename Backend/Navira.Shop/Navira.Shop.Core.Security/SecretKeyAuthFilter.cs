using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Navira.Shop.Core.Security
{
    public class SecretKeyAuthFilter : IAuthorizationFilter
    {
        public static string SecretKey { get; set; } = "";

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var secretCode = filterContext.HttpContext.Request.Headers["secret-key"];
            if (secretCode.All(a => !SecretKey.Equals(a, StringComparison.InvariantCultureIgnoreCase)))
            {
                filterContext.Result = new UnauthorizedResult();
            }
        }
    }
}
