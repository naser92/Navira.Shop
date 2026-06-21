using Microsoft.Extensions.Logging;
using Navira.Shop.Core.Bus;

namespace NaviraShop.Core.Mq
{
    public interface IPublisher
    {
        Task Publish<T>(T eventData) where T : IEvent;
        Task Log(LogEvent log);
        Task Log(string message, LogData data = null, LogLevel level = LogLevel.Information);
        Task Log(string message, LogLevel level);
        Task Log(string message);
        Task Error(string message);
        Task Warning(string message);
        Task Log(Exception exception, string message = "", LogLevel level = LogLevel.Error);
    }
}
