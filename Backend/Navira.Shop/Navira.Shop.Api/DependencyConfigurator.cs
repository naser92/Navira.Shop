using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Infrastructure;
using Navira.Shop.Core.Ioc;

namespace Navira.Shop.Api
{
    public class DependencyConfigurator : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(IServiceCollection serviceCollection, ITypeFinder typeFinder, AppSettings appSettings)
        {
            throw new NotImplementedException();
        }
    }
}
