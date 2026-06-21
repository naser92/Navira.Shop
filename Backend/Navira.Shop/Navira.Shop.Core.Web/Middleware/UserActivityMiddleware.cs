using Microsoft.AspNetCore.Http;
using Navira.Shop.Core.Caching;

namespace Navira.Shop.Core.Web
{
    public class UserActivityMiddleware
    {
        public const string OnlineUsersCacheKey = "pdn.tps.framework.online-user";

        private readonly RequestDelegate _next;
        public UserActivityMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, IStaticCacheManager cacheManager)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var userName = context.User.Identity.Name;
                if (userName != null)
                {
                    var onlineUserInfos = cacheManager.Get(new CacheKey(OnlineUsersCacheKey), () => new Dictionary<string, DateTimeOffset>());
                    if (onlineUserInfos.ContainsKey(userName))
                    {
                        try
                        {
                            onlineUserInfos[userName] = DateTimeOffset.Now;
                        }
                        catch (Exception)
                        {
                            // ignore
                        }
                    }
                    else
                    {
                        onlineUserInfos.Add(userName, DateTimeOffset.Now);
                    }
                    cacheManager.Set(new CacheKey(OnlineUsersCacheKey) { CacheTime = 0 }, onlineUserInfos);
                }
            }
            await _next.Invoke(context);
        }


    }
}