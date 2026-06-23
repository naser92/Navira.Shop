using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Infrastructure;
using System.Reflection;

namespace Navira.Shop.Core.Bus
{
    public static class MassTransitConsumersRegistration
    {
        public static void ConfigMassTransit(this IServiceCollection serviceCollection, AppSettings setting)
        {
            if (setting.RabbitMQ != null && setting.RabbitMQ.AutoConfig)
            {
                Console.WriteLine($"-- configMassTransit start");

                var typeFinder = serviceCollection.BuildServiceProvider().GetRequiredService<ITypeFinder>();
                serviceCollection.AddMassTransit(x =>
                {
                    typeFinder.RegisterConsumers(x);

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        if (!string.IsNullOrWhiteSpace(setting.RabbitMQ.Host))
                        {
                            cfg.Host(new Uri(setting.RabbitMQ.Host), host =>
                            {
                                host.Username(setting.RabbitMQ.Username);
                                host.Password(setting.RabbitMQ.Password);
                            });
                        }
                        cfg.ConfigureJsonSerializerOptions(options =>
                        {
                            options.Converters.Add(new MethodBaseConverter());
                            return options;
                        });

                        typeFinder.RegisterConsumerEvents(context, cfg);

                    });
                });

                //serviceCollection.AddMassTransitHostedService();

                Console.WriteLine($"-- configMassTransit done");
            }
        }

        private static void RegisterConsumers(this ITypeFinder typeFinder, IBusRegistrationConfigurator bussConfigurator)
        {

            var consumers = typeFinder.FindClassesOfType(typeof(IBaseConsumer<>), true);

            try
            {
                foreach (var consumer in consumers)
                {
                    bussConfigurator.AddConsumer(consumer);

                    Console.WriteLine($"--- Consumer {consumer.FullName} added");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private static void RegisterConsumerEvents(this ITypeFinder typeFinder, IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator config)
        {


            var events = typeFinder.FindClassesOfType<IEvent>(true);
            ///events = events.Concat(GetEntityEvents(typeFinder));

            var consumers = typeFinder.FindClassesOfType(typeof(IBaseConsumer<>), true);

            try
            {
                foreach (var evente in events)
                {
                    var consumer = consumers.FirstOrDefault(p => p.GetInterfaces().Any(x => x.GenericTypeArguments.Any(p => p == evente)));


                    if (consumer != null)
                    {
                        var attribute = consumer.GetCustomAttribute<ConsumerConfigAttribute>();
                        var QName = attribute?.Name ?? consumer.FullName;
                        config.ReceiveEndpoint(QName, e =>
                        {
                            e.ConfigureConsumer(context, consumer);
                            if (attribute != null)
                            {
                                if (attribute.ConcurrencyLimit > 0)
                                    e.UseConcurrencyLimit(attribute.ConcurrencyLimit);
                            }
                        });
                        Console.WriteLine($"--- ReceiveEndpoint: {consumer.FullName} added");
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        //private static IEnumerable<Type> GetEntityEvents(this ITypeFinder typeFinder)
        //{
        //    var result = new List<Type>();
        //    // var entities = typeFinder.FindClassesOfType<IAggregateRoot>();
        //    var eventsmodel = typeFinder.FindClassesOfType(typeof(BaseEventModel<>));
        //    //foreach (var entity in entities)
        //    //{
        //    //    result.Add(typeof(EntityCreatedEvent<>).MakeGenericType(entity));
        //    //    result.Add(typeof(EntityDeletedEvent<>).MakeGenericType(entity));
        //    //    result.Add(typeof(EntityUpdatedEvent<>).MakeGenericType(entity));
        //    //}
        //    foreach (var entity in eventsmodel)
        //    {
        //        result.Add(typeof(EntityCreatedEvent<>).MakeGenericType(entity));
        //        result.Add(typeof(EntityDeletedEvent<>).MakeGenericType(entity));
        //        result.Add(typeof(EntityUpdatedEvent<>).MakeGenericType(entity));
        //    }

        //    return result;
        //}
    }
}
