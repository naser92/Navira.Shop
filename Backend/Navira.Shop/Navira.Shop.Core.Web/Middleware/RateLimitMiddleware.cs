using Microsoft.AspNetCore.Http;
using Navira.Shop.Core.Caching;
using Navira.Shop.Core.RateLimit;
using System.Net;

namespace Navira.Shop.Core.Web
{
    public class RateLimitMiddleware
    {
        private const string RateLimitCacheKey = "Navira.Shop.Core:rateLimit";
        private const int DefaultCacheExpireTime = 10;
        private readonly RequestDelegate _next;

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IStaticCacheManager cacheManager)
        {
            var endpoint = context.GetEndpoint();
            var decorator = endpoint?.Metadata?.GetMetadata<LimitRequestsAttribute>();
            if (decorator is null)
            {
                await _next(context);
                return;
            }
            var key = GenerateClientKey(context, cacheManager);
            ClientStatistics clientStatistics = null;
            if (cacheManager.IsSet(key))
            {
                clientStatistics = GetClientStatisticsByKey(key, cacheManager);
            }

            if (clientStatistics != null &&
                   DateTime.Now < clientStatistics.LastSuccessfulResponseTime.AddSeconds(decorator.TimeWindow) &&
                   clientStatistics.NumberOfRequestsCompletedSuccessfully == decorator.MaxRequests)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                return;
            }
            UpdateClientStatisticsStorage(key, decorator.MaxRequests, decorator.TimeWindow, cacheManager);
            await _next(context);
            return;
        }
        private CacheKey GenerateClientKey(HttpContext context, IStaticCacheManager cacheManager)
        {
            var path = context.Request.Path.ToString().Split('/');
            var actionName = path.Length > 1 ? $"{path[path.Length - 2]}" : string.Empty;
            var result = new CacheKey($"{RateLimitCacheKey}:{actionName}_{context.Connection.RemoteIpAddress}") { CacheTime = DefaultCacheExpireTime };
            return result;
        }

        private ClientStatistics GetClientStatisticsByKey(CacheKey key, IStaticCacheManager cacheManager)
        {
            return cacheManager.Get<ClientStatistics>(key);
        }
        private void UpdateClientStatisticsStorage(CacheKey key, int maxRequests, int timeWindow, IStaticCacheManager cacheManager)
        {
            if (!cacheManager.IsSet(key))
            {
                cacheManager.Set(key, new ClientStatistics { LastSuccessfulResponseTime = DateTime.Now, NumberOfRequestsCompletedSuccessfully = 1 });
            }
            else
            {
                var statistics = cacheManager.Get<ClientStatistics>(key);
                var now = DateTime.Now;
                ClientStatistics secondStatistics = null;
                if (statistics.NumberOfRequestsCompletedSuccessfully >= maxRequests
                        || (now - statistics.LastSuccessfulResponseTime).TotalMilliseconds >= TimeSpan.FromSeconds(timeWindow).TotalMilliseconds)
                    secondStatistics = new ClientStatistics { LastSuccessfulResponseTime = now, NumberOfRequestsCompletedSuccessfully = 1 };
                else
                {
                    secondStatistics = new ClientStatistics
                    {
                        LastSuccessfulResponseTime = statistics.LastSuccessfulResponseTime,
                        NumberOfRequestsCompletedSuccessfully = statistics.NumberOfRequestsCompletedSuccessfully + 1
                    };
                }
                cacheManager.Set(key, secondStatistics);
            }
        }
    }
}
