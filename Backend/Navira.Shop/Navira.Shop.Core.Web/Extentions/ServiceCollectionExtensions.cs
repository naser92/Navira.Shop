using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Navira.Shop.Core.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this WebApplicationBuilder builder)
        {
            var serviceCollection = builder.Services;
            var configuration = builder.Configuration;

            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddOpenApi();
            serviceCollection.AddControllers()
              .AddJsonOptions(options =>
              {
                  options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                  options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                  options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                  options.JsonSerializerOptions.Converters.Add(new MethodBaseConverter());
              })
              .AddNewtonsoftJson(options =>
              {
                  options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                  options.SerializerSettings.ContractResolver = new IgnoreMethodBaseResolver();
                  options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                  options.SerializerSettings.Formatting = Formatting.Indented;
              });

            if (builder.Environment.IsNotNull())
                GlobalData.DefaultFileProvider = new CoreFileProvider(builder.Environment.ContentRootPath, builder.Environment.WebRootPath);

            // تنظیم محدودیت حجم فرم‌ها
            serviceCollection.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // 100 MB
            });

            // تنظیم محدودیت حجم Kestrel
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = 104857600; // 100 MB
            });

            var appSettings = new AppSettings();

            configuration.Bind(appSettings);
            serviceCollection.AddSingleton(appSettings);
            serviceCollection.AddAuthentication(appSettings);

            serviceCollection.AddCors();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            if (!appSettings.SystemInfo.IsNull())
            {
                SecretKeyAuthFilter.SecretKey = appSettings.SystemInfo.Id.ToString();
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //var memoryCache = new MemoryCache(new MemoryCacheOptions());
            //serviceCollection.AddSingleton<IMemoryCache>(memoryCache);
            serviceCollection.AddMemoryCache();

            var typeFinder = new WebAppTypeFinder();
            serviceCollection.AddSingleton(typeof(ITypeFinder), typeFinder);

            serviceCollection.AddScoped<IBus, BusControl>();
            serviceCollection.AddScoped<IQueryBus, BusControl>();

            if (appSettings.RedisConfig.Enabled)
            {
                serviceCollection.AddSingleton<ILocker, RedisConnectionWrapper>();
                serviceCollection.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
            }

            if (appSettings.RedisConfig.Enabled && appSettings.RedisConfig.UseCaching)
            {
                serviceCollection.AddSingleton<IStaticCacheManager, RedisCacheManager>();
            }
            else
            {
                serviceCollection.AddSingleton<ILocker, MemoryCacheManager>();
                serviceCollection.AddSingleton<IStaticCacheManager, MemoryCacheManager>();

            }

            serviceCollection.AddSingleton<IAppEngin, AppEngin>();

            serviceCollection.Register<IBaseService>(typeFinder, ServiceLifetime.Scoped);

            serviceCollection.Register(typeFinder, typeof(Core.Persistence.IRepository<,>), ServiceLifetime.Scoped);

            serviceCollection.Register(typeFinder, typeof(IWriteRepository<,>), ServiceLifetime.Scoped);
            serviceCollection.Register(typeFinder, typeof(IQueryRepository<,>), ServiceLifetime.Scoped);

            serviceCollection.Register(typeFinder, typeof(ICommandHandler<>), ServiceLifetime.Scoped);

            serviceCollection.Register(typeFinder, typeof(IQueryHandler<,>), ServiceLifetime.Scoped);

            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("fa");
            serviceCollection.Register(typeFinder, typeof(AbstractValidator<>), ServiceLifetime.Singleton);

            serviceCollection.AddSingleton<IPublisher, Publisher>();
            serviceCollection.AddSingleton<IMessageFactory, MessageFactory>();
            //serviceCollection.RegisterDependencyes(typeFinder, appSettings);

            serviceCollection.RegisterMapConfigs(typeFinder);


            serviceCollection.ConfigMassTransit(appSettings);

            //serviceCollection.AddScoped<UserActivityMonitor>();
            return serviceCollection;
        }

        private static void AddAuthentication(this IServiceCollection service, AppSettings appSettings)
        {
            if (!appSettings.Issuer.IsNullOrWhiteSpace() &&
                !appSettings.Audience.IsNullOrWhiteSpace() &&
                !appSettings.SigningKey.IsNullOrWhiteSpace())
            {

                var secretkey = Encoding.UTF8.GetBytes(appSettings.SigningKey);
                //var encryptionkey = Encoding.UTF8.GetBytes(appSettings.EncryptionKey);

                //Console.WriteLine($"- Config authentication start");
                service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddCookie()
                    .AddJwtBearer(jwtBearerOptions =>
                    {

                        jwtBearerOptions.SaveToken = true;
                        jwtBearerOptions.RequireHttpsMetadata = false;
                        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateActor = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = appSettings.Issuer,
                            ValidAudience = appSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(secretkey),
                            //TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                        };

                    });
                //Console.WriteLine($"- Config authentication done");
            }
            //else
            //Console.WriteLine($"- Authentication do not Config!!");
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
