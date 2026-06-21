using Microsoft.Extensions.DependencyInjection;

namespace  NaviraShop.Core.Mq
{
    public static class ServiceCollectionExtentions
    {
        public static void AddMq(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPublisher, Publisher>();
            serviceCollection.AddSingleton<IMessageFactory, MessageFactory>();
        }
    }
}
