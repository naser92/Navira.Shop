using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Infrastructure;
using Navira.Shop.Core.Ioc;


namespace Navira.Shop.Core.Persistence.EF
{
    public class DependencyConfigurator : IDependencyRegistrar
    {
        public int Order => 0;

        public void Register(IServiceCollection serviceCollection, ITypeFinder typeFinder, AppSettings appSettings)
        {
            serviceCollection.AddScoped<SlowQueryInterceptor>();
        }

    }

}
