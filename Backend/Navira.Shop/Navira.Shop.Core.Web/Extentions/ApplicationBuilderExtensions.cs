using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navira.Shop.Core.Caching;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Infrastructure;
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
            // var publisher = serviceprovider.GetRequiredService<IPublisher>();


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


            //        if (appSettings?.KeycloakConfig != null &&
            //            !string.IsNullOrWhiteSpace(appSettings.KeycloakConfig.BaseUrl) &&
            //            !string.IsNullOrWhiteSpace(appSettings.KeycloakConfig.Realm) &&
            //            !string.IsNullOrWhiteSpace(appSettings.KeycloakConfig.ClientId))
            //        {
            //            app.UseAuthentication();

            //            var nonRedirectPaths = new[]
            //            {
            //                "/health",
            //                "/metrics"
            //};

            //            app.Use(async (context, next) =>
            //            {
            //                await next();

            //                bool isMatch = nonRedirectPaths.Any(path =>
            //                    context.Request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase));

            //                if (isMatch && context.Response.StatusCode == StatusCodes.Status302Found)
            //                {
            //                    context.Response.Clear();
            //                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //                    context.Response.Headers.WWWAuthenticate = "Bearer";
            //                }
            //            });

            //            app.UseAuthorization();
            //        }
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.CallAppConfigs(typeFinder, appSettings);

            return app;
        }

        private static void CallAppConfigs(this WebApplication app, ITypeFinder typeFinder, AppSettings appSettings)
        {

            var configServices = typeFinder.FindClassesOfType<IAppConfigService>();

            var instances = configServices
                .Select(configService => (IAppConfigService)Activator.CreateInstance(configService))
                .OrderBy(configService => configService.Order);


            foreach (var configService in instances)
            {
                configService.Config(app, typeFinder, appSettings);
            }
        }
    }
}
