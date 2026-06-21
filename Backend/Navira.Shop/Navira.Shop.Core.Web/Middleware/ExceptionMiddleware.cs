using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Results;
using Navira.Shop.Core.Security;
using NaviraShop.Core.Mq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestEase;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Navira.Shop.Core.Web
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        // private readonly IPublisher _publisher;
        private readonly AppSettings _appSettings;
        private readonly Stopwatch _stopWatch;

        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
        public ExceptionMiddleware(RequestDelegate next, AppSettings appSettings)//
        {
            _appSettings = appSettings;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, IPublisher publisher, IAppEngin appEngin)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(httpContext);
            }
            catch (ResultException ex)
            {
                await HandleResultException(httpContext, ex, publisher);
            }
            catch (ApiException ex)
            {
                await HandleApiException(httpContext, ex, publisher, appEngin);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex, publisher, appEngin);
            }
            finally
            {
                stopwatch.Stop();
                if (stopwatch.Elapsed.TotalSeconds > 10)
                    await publisher.Log("Diagnostics : Slow process ", new(QueryTime: stopwatch.Elapsed), LogLevel.Warning);
            }

        }
        private async Task HandleApiException(HttpContext context, ApiException exception, IPublisher publisher, IAppEngin appEngin)
        {
            var result = "خطای سرور";

            if (_appSettings.ShowErrorMessage)
                result = $"خطای سرویس خارجی: {exception.RequestUri} : {exception.Content} ";

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            await publisher.Error(result);

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                Error = true,
                Message = result,
                appEngin.TraceId
            }, serializerSettings), Encoding.UTF8);
        }
        private async Task HandleResultException(HttpContext context, ResultException exception, IPublisher publisher)
        {
            var result = exception.Result.Result;
            var ex = (Exception)exception;
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            while (ex.InnerException != null) { ex = ex.InnerException; }
            await publisher.Log(exception: ex, message: result.Message);

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result, serializerSettings), Encoding.UTF8);
        }
        private async Task HandleException(HttpContext context, Exception exception, IPublisher publisher, IAppEngin appEngin)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            while (exception.InnerException != null) { exception = exception.InnerException; }
            await publisher.Log(exception, exception.Message);

            var message = "خطای سرور";

            if (exception.Message.Contains("unique index") || exception.Message.Contains("unique constraint"))
            {
                message = "ثبت اطلاعات متوقف شد. لطفا اطلاعات وارد شده را بررسی و مجددا ارسال کنید.";
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (_appSettings.ShowErrorMessage)
                    message += exception.Message;
            }

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                Error = true,
                StatusCode = context.Response?.StatusCode,
                Message = message,
                appEngin.TraceId
            }, serializerSettings), Encoding.UTF8);
        }

    }

}
