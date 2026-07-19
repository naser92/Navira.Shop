using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Navira.Shop.Core.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // not logged in
                context.Result = new JsonResult(new
                {
                    message = "Unauthorized",
                    error = true,

                })
                { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            //DenyBlackListToken(context);
        }

        //    private void DenyBlackListToken(AuthorizationFilterContext context)
        //    {

        //        var svc = context.HttpContext.RequestServices;
        //        var CacheManager = svc.GetService<IStaticCacheManager>();
        //        var token = GetTokenFromHeader(context.HttpContext);
        //        var handler = new JwtSecurityTokenHandler();
        //        var jwtSecurityToken = handler.ReadJwtToken(token);
        //        var jtid = jwtSecurityToken.Claims.Single(s => s.Type == "jti").Value;
        //        var exp = long.Parse(jwtSecurityToken.Claims.Single(s => s.Type == "exp").Value);

        //        var isExpired = DateTimeOffset.UtcNow > DateTimeOffset.Parse("1970-01-01T00:00:00Z").AddSeconds(exp);

        //        if (isExpired || CacheManager.IsSet(new CacheKey($"{CachkeyConsts.BlackListTokenCacheKey}{jtid}")))
        //        {
        //            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //            context.Result = new JsonResult(new
        //            {
        //                message = "Unauthorized - Token Expired.",
        //                error = true,
        //            })
        //            { StatusCode = StatusCodes.Status401Unauthorized };
        //        }
        //    }
        //}

        // private string GetTokenFromHeader(HttpContext context)
        //    {
        //        string authorizationHeader = context?.Request?.Headers["Authorization"];
        //        if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
        //        {
        //            return authorizationHeader.Substring($"{AuthenticationSchemes} ".Length);
        //            // Process the token
        //        }
        //        return null;
        //    }
    }

}
