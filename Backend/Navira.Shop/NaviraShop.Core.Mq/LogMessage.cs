using Microsoft.Extensions.Logging;
using Navira.Shop.Core.Bus;

namespace NaviraShop.Core.Mq
{

    public class LogEvent : IEvent
    {
        public DateTime TimeStamp { get; set; }
        public LogLevel Level { get; set; } = LogLevel.Information;
        public string MessageTemplate { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public CustomExpetionItem Properties { get; set; }
        public string Request { get; set; }
    }

    public class CustomExpetionItem
    {
        public string Name { get; set; }
        public Guid? UserId { get; set; }
        //public string NationalCode { get; set; }
        //public Guid? ParentUserId { get; set; }
        public string UserName { get; set; }
        public string RequestPath { get; set; }
        public string ApplicationName { get; set; }
        public bool IsAdmin { get; set; }
        //public Guid? UserContainerId { get; set; }
        //public Guid? ParentUserContainerId { get; set; }
        public Guid? Branchid { get; set; }
        //public Guid? CompanyId { get; set; }
        public Guid? TraceId { get; set; }
        public TimeSpan? Time { get; set; }
        public int? ResponseStatusCode { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Method { get; set; }
        public string Database { get; set; }
        public string Query { get; set; }
        public double? QueryTime { get; set; }
        public string Content { get; set; }
    }
}
