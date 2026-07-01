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
            serviceCollection.Register(typeFinder, typeof(IDomainEventHandler<>), ServiceLifetime.Scoped);
            serviceCollection.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>(); // needs an impl — see gap 3
        }

        public int Order => 2;
    }

}
