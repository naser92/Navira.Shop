using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Security;

namespace NaviraShop.Core.Mq
{
    public class MessageFactory : IMessageFactory
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppSettings _appSettings;
        private readonly IAppEngin _appEngine;
        //private readonly IRequestTracker _requestTracker;

        public MessageFactory(
            IHttpContextAccessor contextAccessor,
            AppSettings appSettings
,
            IAppEngin appEngine
            // IRequestTracker requestTracker = null
            )
        {
            _contextAccessor = contextAccessor;

            _appSettings = appSettings;
            _appEngine = appEngine;
            //_requestTracker = requestTracker;
        }
        public async Task<LogEvent> LogMessage(string message, LogData Data = null, LogLevel level = LogLevel.Information)
        {

            var properties = await GetInfo(Data);
            return new LogEvent()
            {
                Message = message,
                TimeStamp = DateTime.Now,
                Level = level,
                Properties = properties,
            };
        }
        public async Task<LogEvent> LogMessage(string message, LogLevel level = LogLevel.Information)
        {

            var properties = await GetInfo();
            return new LogEvent()
            {
                Message = message,
                TimeStamp = DateTime.Now,
                Level = level,
                Properties = properties,
            };
        }
        public async Task<LogEvent> LogMessage(Exception exception, string message = "", LogLevel level = LogLevel.Error)
        {
            var properties = await GetInfo();
            return new LogEvent()
            {
                Exception = exception?.ToString(),
                Level = level,
                Message = message,
                TimeStamp = DateTime.Now,
                Properties = properties
            };
        }
        public async Task<CustomExpetionItem> GetInfo(LogData? Data = null)
        {
            var httpContext = _contextAccessor.HttpContext;
            TokenSensitiveData sensitiveData = null;
            string userName = "";
            string requestBody = "";
            string responseBody = "";
            try
            {

                (sensitiveData, userName) = httpContext.GetSensitiveDate();
                requestBody = await httpContext.GetRequestBody();
                responseBody = httpContext.GetResponseBody();

            }
            catch (Exception)
            {
                responseBody = "cannot read Sensitive data";
            }
            return new CustomExpetionItem()
            {
                ApplicationName = _appSettings.SystemInfo.Name,
                TraceId = _appEngine.TraceId,

                UserId = sensitiveData?.UserId,
                //ParentUserId = sensitiveData?.ParentUserId,
                //NationalCode = sensitiveData?.Nationalcode,
                IsAdmin = sensitiveData?.IsAdmin ?? false,
                //UserContainerId = sensitiveData?.UserContainerId,
                //ParentUserContainerId = sensitiveData?.ParentUserContainerId,
                //CompanyId = sensitiveData?.CompanyId,
                UserName = userName,
                RequestPath = httpContext?.Request?.Path.Value,
                ResponseStatusCode = httpContext?.Response?.StatusCode,
                Request = requestBody,
                Response = responseBody,
                Method = httpContext?.Request?.Method,
                //Name = sensitiveData?.Name,


                QueryTime = Data?.QueryTime?.TotalMicroseconds,
                Query = Data?.Query,
                Database = Data?.Database,
                Content = Data?.Content,

            };
        }
        //private async Task<string> GetContextStringfy()
        //{
        //    var context = _contextAccessor.HttpContext;
        //    var request = await LogRequest(context.Request);
        //    var response = await LogResponse(context.Response);;

        //    return $"{{\"request\":{{{request}}},\"response\":{{{response}}}}}";
        //}


        //private async Task<string> LogRequest(HttpRequest httpRequest)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();

        //    AppendMethod(httpRequest, stringBuilder);
        //    AppendPath(httpRequest, stringBuilder);
        //    AppendContentType(httpRequest, stringBuilder);
        //    AppendHeaders(httpRequest, stringBuilder);
        //    //AppendParams(httpRequest, stringBuilder);
        //    AppendQueries(httpRequest, stringBuilder);
        //    await AppendBody(httpRequest, stringBuilder);

        //    return stringBuilder.ToString();
        //}

        //private void AppendMethod(HttpRequest httpRequest, StringBuilder stringBuilder)
        //{
        //    stringBuilder.AppendLine($"\"method\":\"{httpRequest.Method}\",");
        //}

        //private void AppendPath(HttpRequest httpRequest, StringBuilder stringBuilder)
        //{
        //    stringBuilder.AppendLine($"\"path\":\"{httpRequest.Path.Value}\",");
        //}

        //private void AppendContentType(HttpRequest httpRequest, StringBuilder stringBuilder)
        //{
        //    string contentType = httpRequest.ContentType;

        //    if (!string.IsNullOrEmpty(contentType))
        //    {
        //        stringBuilder.AppendLine($"\"contentType\":\"{contentType}\",");
        //    }
        //}

        //private void AppendHeaders(HttpRequest httpRequest, StringBuilder stringBuilder)
        //{
        //    AppendCollection(httpRequest.Headers, "headers", stringBuilder);
        //}

        //private void AppendParams(HttpRequest httpRequest, StringBuilder stringBuilder)
        //{
        //    AppendCollection(httpRequest.RouteValues, "Params", stringBuilder);
        //}

        //private void AppendQueries(HttpRequest httpRequest, StringBuilder stringBuilder)
        //{
        //    AppendCollection(httpRequest.Query, "queries", stringBuilder);
        //}

        //private void AppendCollection<TKey, TValue>(
        //    IEnumerable<KeyValuePair<TKey, TValue>> collection,
        //    string collectionName,
        //    StringBuilder stringBuilder)
        //{
        //    if (collection.Any())
        //    {
        //        stringBuilder.AppendLine($"\"{collectionName}\":{{");

        //        foreach ((TKey key, TValue value) in collection)
        //        {
        //            stringBuilder.AppendLine($"\"{key}\":\"{value.ToString().Replace("\"","\\\"")}\",");
        //        }
        //        stringBuilder.AppendLine($"}},");
        //    }
        //}

        //private async Task AppendBody(HttpRequest httpRequest, StringBuilder stringBuilder)
        //{
        //    httpRequest.EnableBuffering();
        //    var  bodyBytes=new Memory<byte>();
        //    await httpRequest.Body.ReadAsync(bodyBytes);

        //    if (bodyBytes.Length > 0)
        //    {
        //        string bodyText = Encoding.UTF8.GetString(bodyBytes.ToArray());
        //        stringBuilder.AppendLine($"\"body\":\"{bodyText}\",");
        //    }

        //    httpRequest.Body.Seek(0, SeekOrigin.Begin);
        //}


        //public async Task InvokeAsync(HttpContext httpContext)
        //{
        //    HttpResponse httpResponse = httpContext.Response;
        //    Stream originalBody = httpResponse.Body;

        //    try
        //    {
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            httpResponse.Body = memoryStream;

        //            //await requestDelegate(httpContext);
        //            await LogResponse(httpContext.Response, originalBody, memoryStream);
        //        }
        //    }
        //    finally
        //    {
        //        httpContext.Response.Body = originalBody;
        //    }
        //}

        //private async Task<string> LogResponse(HttpResponse httpResponse)//, Stream originalBody, MemoryStream memoryStream)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();

        //    AppendStatusCode(httpResponse, stringBuilder);
        //    AppendContentType(httpResponse, stringBuilder);
        //    //await AppendBody(httpResponse, stringBuilder);//, originalBody, memoryStream);

        //    return stringBuilder.ToString();
        //}

        //private void AppendStatusCode(HttpResponse httpResponse, StringBuilder stringBuilder)
        //{
        //    stringBuilder.AppendLine($"\"statusCode\":\"{httpResponse.StatusCode}\",");
        //}

        //private void AppendContentType(HttpResponse httpResponse, StringBuilder stringBuilder)
        //{
        //    string contentType = httpResponse.ContentType;

        //    if (!string.IsNullOrEmpty(contentType))
        //    {
        //        stringBuilder.AppendLine($"\"contentType\":\"{contentType}\",");
        //    }
        //}

        //private async Task AppendBody(HttpResponse httpResponse, StringBuilder stringBuilder)//, Stream originalBody, MemoryStream memoryStream)
        //{
        //    httpResponse.Body.Seek(0, SeekOrigin.Begin);

        //    StreamReader streamReader = new StreamReader(httpResponse.Body);
        //    string bodyText = await streamReader.ReadToEndAsync();

        //    if (bodyText.Length > 0)
        //        stringBuilder.AppendLine($"\"body\":\"{bodyText}\",");

        //    httpResponse.Body.Seek(0, SeekOrigin.Begin);
        //   // await memoryStream.CopyToAsync(originalBody);
        //}
    }


}
