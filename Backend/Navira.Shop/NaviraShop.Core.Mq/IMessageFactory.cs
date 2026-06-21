using Microsoft.Extensions.Logging;

namespace NaviraShop.Core.Mq
{
    public interface IMessageFactory
    {
        Task<LogEvent> LogMessage(string message, LogLevel level = LogLevel.Information);
        Task<LogEvent> LogMessage(Exception exception, string message = "", LogLevel level = LogLevel.Error);
        Task<CustomExpetionItem> GetInfo(LogData data = null);
        Task<LogEvent> LogMessage(string message, LogData data = null, LogLevel level = LogLevel.Information);
    }
}
