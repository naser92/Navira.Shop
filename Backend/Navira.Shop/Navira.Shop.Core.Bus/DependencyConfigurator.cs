using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Infrastructure;
using Navira.Shop.Core.Ioc;


namespace Navira.Shop.Core.Bus
{
    public class DependencyConfigurator : IDependencyRegistrar
    {

        public void Register(IServiceCollection serviceCollection, ITypeFinder typeFinder, AppSettings appSettings)
        {

        }

        public int Order => 2;
    }

}
