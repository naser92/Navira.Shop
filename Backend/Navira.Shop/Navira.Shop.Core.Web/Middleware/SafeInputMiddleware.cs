using Microsoft.AspNetCore.Http;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Navira.Shop.Core.Web
{
    /// <summary>
    ///  اعتبار سنجی داده ورودی از نظر کاراکتر ها
    /// </summary>
    public class SafeInputMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSetting;
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
        public SafeInputMiddleware(RequestDelegate next, AppSettings appSetting)
        {
            _next = next;
            _appSetting = appSetting;
        }

        private string FixJObjectData(string json)
        {
            object? obj = null;
            try
            {
                obj = JsonConvert.DeserializeObject(json);
                FixJObjectData(obj);
            }
            catch (Exception ex)
            {
                return json;
            }
            return JsonConvert.SerializeObject(obj);
        }
        private void FixJObjectData(object? obj)
        {
            if (obj is null) return;
            if (obj is JArray)
            {
                JArray array = (JArray)obj;
                for (int i = 0; i < array.Count(); ++i)
                    FixJObjectData(array[i]);
            }
            else if (obj is JObject)
            {
                JObject json = (JObject)obj;
                var properties = json.Properties();
                if (properties == null) return;
                foreach (var property in properties.ToList())
                {
                    try
                    {
                        if (property.Type != JTokenType.Object && property.Type != JTokenType.Array && string.IsNullOrWhiteSpace(property.Value.Value<string>()))
                        {
                            json.Remove(property.Name);
                        }
                        else
                        {
                            FixJObjectData(property);
                        }
                    }
                    catch (Exception ex) { }
                }
            }
        }
        private bool ChckIfHasInvalidValue(string value)
        {
            var fieldValues = Regex.Matches(value.ToLower(), ": ?(?<value>[^({|}|,)]+)")
                            .Select(x => x.Groups["value"].Value);

            string[] sqlCheckList =
            {
                            "--", ";--", ";", "//", "@@", "alter", "begin",
                            "cast", "create", "cursor", "declare", "delete", "drop",  "exec", "execute", "fetch",
                            "insert",
                            "kill", "select", "sys", "sysobjects", "syscolumns", "table", "update",
                            "<", ">", "!", "?", "#", "&", "script"
                        };

            //var valid = sqlCheckList.Where(item => fieldValues.Any(f => f.Contains(item.ToLower())));
            var containInvalidValue = sqlCheckList.Where(item => fieldValues.Any(value => value == item.ToLower()));


            return containInvalidValue.Any();
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value != null && _appSetting.IgnoreCheckPath.All(a => a != "*"))
            {
                if (!_appSetting.IgnoreCheckPath.Any(a => context.Request.Path.Value.ToLower().Contains(a.ToLower())))
                {
                    context.Request.EnableBuffering();
                    var requestReader = new StreamReader(context.Request.Body);
                    var requestContent = await requestReader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                    if (requestContent.IsNotNullOrWhiteSpace())
                    {

                        if (ChckIfHasInvalidValue(requestContent))
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                            context.Response.ContentType = "application/json; charset=utf-8";

                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                            {
                                Error = true,
                                StatusCode = context.Response?.StatusCode,
                                Message = "مقدار ورودی شامل کارکتر و یا کلمات نامعتبر است.",
                                TraceId = Guid.NewGuid(),
                            }, serializerSettings), Encoding.UTF8);
                            return;
                        }
                        try
                        {
                            var result = FixJObjectData(requestContent.ApplyCorrectYeKe().FixNumberValue());
                            var requestData = Encoding.UTF8.GetBytes(result);
                            context.Request.Body = new MemoryStream(requestData);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}