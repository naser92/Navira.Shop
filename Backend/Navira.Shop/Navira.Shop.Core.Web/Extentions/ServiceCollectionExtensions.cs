using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Caching;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Infrastructure;
using Navira.Shop.Core.Ioc;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Core.Security;
using Navira.Shop.Core.Service;
using NaviraShop.Core.Mq;
using System.Globalization;
using System.Text;

namespace Navira.Shop.Core.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;
            var environment = builder.Environment;


            // -------------------------------------------------
            // Configuration
            // -------------------------------------------------

            var appSettings = configuration.Get<AppSettings>() ?? new AppSettings();
            services.AddSingleton(appSettings);


            // -------------------------------------------------
            // ASP.NET Core
            // -------------------------------------------------

            services.ConfigureMvc();
            services.ConfigureRequestLimits(builder);
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddMemoryCache();
            services.AddOpenApi();

            // -------------------------------------------------
            // File Provider
            // -------------------------------------------------
            GlobalData.DefaultFileProvider = new CoreFileProvider(
                                                environment.ContentRootPath,
                                                environment.WebRootPath);


            // -------------------------------------------------
            // Authentication
            // -------------------------------------------------

            services.AddKeycloakAuthentication(appSettings, environment);


            // -------------------------------------------------
            // Authorization
            // -------------------------------------------------

            services.AddPermissionAuthorization();


            // -------------------------------------------------
            // Infrastructure
            // -------------------------------------------------

            var typeFinder = new WebAppTypeFinder();
            services.AddSingleton(typeof(ITypeFinder), typeFinder);

            services.ConfigureCaching(appSettings);

            services.ConfigureRepositories(typeFinder);

            services.ConfigureMessaging(appSettings);

            services.ConfigureValidation(typeFinder);

            services.ConfigureMapping(typeFinder);

            services.ConfigureApplication(appSettings, typeFinder);

            // -------------------------------------------------
            // Dependency Injection
            // -------------------------------------------------

            services.RegisterDependencyes(typeFinder, appSettings);


            // -------------------------------------------------
            // Misc
            // -------------------------------------------------

            if (!appSettings.SystemInfo.IsNull())
            {
                SecretKeyAuthFilter.SecretKey = appSettings.SystemInfo.Id.ToString();
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("fa");

            return services;


        }

        private static void ConfigureApplication(this IServiceCollection services, AppSettings appSettings, WebAppTypeFinder typeFinder)
        {
            services.AddSingleton<IAppEngin, AppEngin>();
            services.AddSingleton<IPermissionProvider, RedisPermissionProvider>();


            services.Register<IBaseService>(typeFinder, ServiceLifetime.Scoped);


            services.Register(typeFinder, typeof(ICommandHandler<>), ServiceLifetime.Scoped);

            services.Register(typeFinder, typeof(IQueryHandler<,>), ServiceLifetime.Scoped);
        }

        private static void ConfigureMapping(this IServiceCollection services, WebAppTypeFinder typeFinder)
        {
            services.RegisterMapConfigs(typeFinder);

        }

        private static void ConfigureValidation(this IServiceCollection services, WebAppTypeFinder typeFinder)
        {

            services.Register(typeFinder, typeof(AbstractValidator<>), ServiceLifetime.Singleton);

        }

        private static void ConfigureMessaging(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddScoped<IBus, BusControl>();
            services.AddScoped<IQueryBus, BusControl>();
            services.AddSingleton<IPublisher, Publisher>();
            services.AddSingleton<IMessageFactory, MessageFactory>();

            services.ConfigMassTransit(appSettings);

        }

        private static void ConfigureRepositories(this IServiceCollection service, WebAppTypeFinder typeFinder)
        {

            service.Register(typeFinder, typeof(Core.Persistence.IRepository<,>), ServiceLifetime.Scoped);

            service.Register(typeFinder, typeof(IWriteRepository<,>), ServiceLifetime.Scoped);
            service.Register(typeFinder, typeof(IQueryRepository<,>), ServiceLifetime.Scoped);

        }

        private static void ConfigureCaching(this IServiceCollection service, AppSettings appSettings)
        {
            service.AddMemoryCache();

            if (appSettings.RedisConfig.Enabled)
            {
                service.AddSingleton<ILocker, RedisConnectionWrapper>();
                service.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
            }

            if (appSettings.RedisConfig.Enabled && appSettings.RedisConfig.UseCaching)
            {
                service.AddSingleton<IStaticCacheManager, RedisCacheManager>();
            }
            else
            {
                service.AddSingleton<ILocker, MemoryCacheManager>();
                service.AddSingleton<IStaticCacheManager, MemoryCacheManager>();

            }
        }


        private static void RegisterDependencyes(this IServiceCollection serviceCollection, ITypeFinder typeFinder, AppSettings appSettings)
        {

            Console.WriteLine($"- dependency config start");
            var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>();

            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
            {
                Console.WriteLine($"-- {dependencyRegistrar.GetType().FullName} registering");
                dependencyRegistrar.Register(serviceCollection, typeFinder, appSettings);
                Console.WriteLine($"-- {dependencyRegistrar.GetType().FullName} registered");
            }
            Console.WriteLine($"- dependency config done");
        }

        private static void RegisterMapConfigs(this IServiceCollection serviceCollection, ITypeFinder typeFinder)
        {

            //Console.WriteLine($"- Map config start");
            var configs = typeFinder.FindClassesOfType<IMapConfig>();

            var instances = configs
                .Select(config => (IMapConfig)Activator.CreateInstance(config))
                .OrderBy(config => config.Order);

            //register all provided dependencies
            foreach (var config in instances)
            {
                //Console.WriteLine($"-- {config.GetType().FullName} registering");
                config.Config();
                //Console.WriteLine($"-- {config.GetType().FullName} registered");
            }
            //Console.WriteLine($"0 Map config done");
        }

        public static void Register(this IServiceCollection serviceCollection, ITypeFinder typeFinder, Type type, ServiceLifetime serviceLifetime)
        {

            //Console.WriteLine($"- Register type:{type.FullName} Start");
            var services = typeFinder.FindClassesOfType(type);


            foreach (var service in services)
            {
                var types = new List<Type>();
                if (type.IsClass && service.BaseType != null && type.Name == service.BaseType.Name)
                    types.Add(service.BaseType);
                else
                {
                    var interfaces = service.GetInterfaces();
                    types = interfaces.Except(interfaces.SelectMany(t => t.GetInterfaces())).ToList();
                }

                foreach (var itype in types)
                {
                    if (services.Any(x => !x.Equals(service) && itype.IsAssignableFrom(x)))
                        continue;

                    serviceCollection.Add(new ServiceDescriptor(itype, service, serviceLifetime));
                    //Console.WriteLine($"-- {itype.FullName}\n ---> --\n{service.FullName} registerd");
                }

            }
            //Console.WriteLine($"- Register type:{type.FullName} done");
        }

        public static void Register<T>(this IServiceCollection serviceCollection, ITypeFinder typeFinder, ServiceLifetime serviceLifetime)
        {

            serviceCollection.Register(typeFinder, typeof(T), serviceLifetime);

        }

        public static void AddFactory<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TService, TImplementation>();
            services.AddSingleton<Func<TService>>(x => x.GetService<TService>);

        }

    }
}
