using Microsoft.Extensions.DependencyInjection;

namespace Navira.Shop.Core.HealthCheck
{
    public interface IHealthCheckRegistrar
    {
        void Register(IHealthChecksBuilder builder);
    }
}
