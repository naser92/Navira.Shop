using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.HealthCheck;

namespace Navira.Shop.Core.Caching
{
    public class HealthCheckRegistrar : IHealthCheckRegistrar
    {
        private readonly AppSettings _appSettings;

        public HealthCheckRegistrar(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void Register(IHealthChecksBuilder builder)
        {
            if (_appSettings.RedisConfig?.Enabled == true)
            {
                var redisConnString = _appSettings.RedisConfig.ConnectionString;
                if (!string.IsNullOrWhiteSpace(redisConnString))
                {
                    builder.AddRedis(
                        redisConnectionString: redisConnString,
                        name: "Redis",
                        tags: new[] { "redis", "southbound", "readiness" });
                }
            }

        }
    }
}
