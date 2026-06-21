using Microsoft.AspNetCore.Http;
using System.Text;

namespace Navira.Shop.Core.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetResponseBody(this HttpContext context)
        {
            try
            {
                if (context.IsNotNull() && context.Response.IsNotNull() && context.Response.ContentLength.IsPositive())
                    using (var stream = context.Response.Body)
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                return "";
            }
            catch (Exception ex)
            {
            }
            return "";
        }
        public static async Task<string> GetRequestBody(this HttpContext context)
        {
            try
            {
                if (context.IsNotNull() && context.Request.IsNotNull())
                {
                    var request = context.Request;
                    HttpRequestRewindExtensions.EnableBuffering(request);
                    if ((request.Method == HttpMethods.Post || request.Method == HttpMethods.Put) && request.Body.Length > 0)
                    {
                        request.Body.Position = 0;
                        using (var reader = new StreamReader(request.Body
                            , encoding: Encoding.UTF8
                            , detectEncodingFromByteOrderMarks: false
                            , leaveOpen: true))
                        {
                            return await reader.ReadToEndAsync();
                        }
                    }
                    else if ((request.Method == HttpMethods.Get || request.Method == HttpMethods.Delete))
                    {
                        return request.QueryString.ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
            }
            return "";
        }
    }
}
