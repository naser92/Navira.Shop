using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navira.Shop.Core.Caching;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Infrastructure;
using NaviraShop.Core.Mq;
using Scalar.AspNetCore;

namespace Navira.Shop.Core.Web
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication ConfigureApplicationBuilder(this WebApplicationBuilder webApplicationBuilder)
        {
            var app = webApplicationBuilder.Build();
            var serviceprovider = app.Services;
            var staticCacheManager = serviceprovider.GetRequiredService<IStaticCacheManager>();
            var appSettings = serviceprovider.GetRequiredService<AppSettings>();
            var typeFinder = serviceprovider.GetRequiredService<ITypeFinder>();
            var publisher = serviceprovider.GetRequiredService<IPublisher>();


            if (appSettings?.Scalar?.Enabled == true)
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options
                    .WithTheme(ScalarTheme.Kepler)
                    .WithDarkModeToggle(true)
                    .WithClientButton(true);
                    options.Title = appSettings?.SystemInfo?.Title;
                });

            }

            //container.BeginScope();
            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();


            app.UseMiddleware<SafeInputMiddleware>();
            //Console.WriteLine($"-- SafeInputMiddleware configured");

            app.UseMiddleware<ExceptionMiddleware>();
            // Console.WriteLine($"-- ExceptionMiddleware configured");

            app.UseMiddleware<UserActivityMiddleware>();
            //Console.WriteLine($"-- ExceptionMiddleware configured");

            //app.UseContainerScope();
            app.UseRouting();
            app.UseCors(policyBuilder =>
            {
                policyBuilder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition");
                if (!appSettings.SystemInfo.IsNull() && !appSettings.SystemInfo.AllowOrigins.IsNullOrEmpty())
                    policyBuilder.WithOrigins(appSettings.SystemInfo.AllowOrigins);
                else
                    policyBuilder.AllowAnyOrigin();
            });

            //Console.WriteLine($"-- UseRouting configured");

            if (!appSettings.Issuer.IsNullOrWhiteSpace() &&
               !appSettings.Audience.IsNullOrWhiteSpace() &&
               !appSettings.SigningKey.IsNullOrWhiteSpace())
            {

                // Console.WriteLine($"- Config authentication start");
                app.UseAuthentication();
                var nonRedirectPaths = new[] {
                            "/health",
                            "/metrics"
                        };
                app.Use(async (context, next) =>
                {
                    await next();
                    bool isMatch = nonRedirectPaths.Any(path =>
                        context.Request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase));

                    if (isMatch && context.Response.StatusCode == StatusCodes.Status302Found)
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        // هدر زیر برای اینکه PRTG بفهمد باید Basic Auth بفرستد ضروری است
                        context.Response.Headers.WWWAuthenticate = "Basic realm=\"API Security\"";
                    }
                });

                app.UseAuthorization();
            }
            //else
            // Console.WriteLine($"- Authentication not configured !!");


            //Console.WriteLine($"- Root app config End");

            app.MapControllers();
            //app.MapHealthChecks("/config/status/v2", new HealthCheckOptions
            //{
            //    Predicate = check => !check.Name.Equals("masstransit-bus", StringComparison.OrdinalIgnoreCase),
            //    ResponseWriter = async (context, report) =>
            //    {
            //        var publisher = context.RequestServices
            //            .GetRequiredService<IPublisher>();
            //        context.Response.ContentType = "application/json";

            //        var response = new
            //        {
            //            status = report.Status,
            //            checks = report.Entries.Select(e => new
            //            {
            //                name = e.Key,
            //                status = e.Value.Status,
            //                data = e.Value.Data
            //            })
            //        };
            //        await publisher.Log("Diagnostics: Health check (v2)", new LogData(Content: response.Serialize()));
            //        await context.Response.WriteAsJsonAsync(response);
            //    }
            //});
            app.CallAppConfigs(typeFinder, appSettings);
            publisher.Log($"{appSettings?.SystemInfo?.Name} started ");
            return app;
        }

        private static void CallAppConfigs(this WebApplication app, ITypeFinder typeFinder, AppSettings appSettings)
        {

            var configServices = typeFinder.FindClassesOfType<IAppConfigService>();

            var instances = configServices
                .Select(configService => (IAppConfigService)Activator.CreateInstance(configService))
                .OrderBy(configService => configService.Order);

            //register all provided dependencies
            foreach (var configService in instances)
            {
                // Console.WriteLine($"--- {configService.GetType().FullName} Config");
                configService.Config(app, typeFinder, appSettings);
                // Console.WriteLine($"--- {configService.GetType().FullName} Configed");
            }
            // Console.WriteLine($"-- Service app config done");
        }
    }
}
