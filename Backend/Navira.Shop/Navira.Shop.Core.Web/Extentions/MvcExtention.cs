using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Navira.Shop.Core.Web
{
    public static class MvcExtention
    {
        public static IServiceCollection ConfigureMvc(this IServiceCollection services)
        {
            services
               .AddControllers()
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.DefaultIgnoreCondition =
                       JsonIgnoreCondition.WhenWritingNull;

                   options.JsonSerializerOptions.PropertyNamingPolicy =
                       JsonNamingPolicy.CamelCase;

                   options.JsonSerializerOptions.DictionaryKeyPolicy =
                       JsonNamingPolicy.CamelCase;

                   options.JsonSerializerOptions.Converters
                       .Add(new MethodBaseConverter());
               })
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.NullValueHandling =
                       NullValueHandling.Ignore;

                   options.SerializerSettings.ContractResolver =
                       new CamelCasePropertyNamesContractResolver();

                   options.SerializerSettings.Formatting =
                       Formatting.Indented;
               });

            return services;
        }

        public static IServiceCollection ConfigureRequestLimits(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600;
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 104857600;
            });

            return services;
        }
    }
}
