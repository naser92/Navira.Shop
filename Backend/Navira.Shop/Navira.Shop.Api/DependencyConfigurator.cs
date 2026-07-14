using Microsoft.EntityFrameworkCore;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Infrastructure;
using Navira.Shop.Core.Ioc;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Core.Security;
using Navira.Shop.Infrastructure.Persistence;

namespace Navira.Shop.Api
{
    public class DependencyConfigurator : IDependencyRegistrar
    {
        public int Order => int.MaxValue;

        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            services.AddDbContext<WriteDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetRequiredService<SlowQueryInterceptor>());
                options.UseSqlServer(appSettings.GetConnectionStrings("WriteConnection"),
                    b =>
                    {
                        b.MigrationsAssembly("Navira.Shop.Infrastructure.Persistence");
                        b.CommandTimeout(1800);
                        b.TranslateParameterizedCollectionsToConstants();

                    });
                options.EnableSensitiveDataLogging();
            });
            //services.AddDbContext<QueryDbContext>((sp, options) =>
            //{
            //    options.AddInterceptors(sp.GetRequiredService<SlowQueryInterceptor>());
            //    options.UseSqlServer(appSettings.GetConnectionStrings("ReadConnection"),
            //        b =>
            //        {
            //            b.MigrationsAssembly("PDN.TPS.Taxpayer.Query.DataContext");
            //            b.CommandTimeout(1800);
            //            b.TranslateParameterizedCollectionsToConstants();
            //        });
            //});
            var serviceProvicder = services.BuildServiceProvider();
            services.AddScoped<WriteDbContext>();
            services.AddScoped(typeof(IUnitOfWork), p => p.GetService<WriteDbContext>());

            if (!appSettings.SystemInfo.IsNull())
            {
                SecretKeyAuthFilter.SecretKey = appSettings.SystemInfo.Id.ToString();
            }

        }
    }
}
