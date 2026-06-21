using MassTransit;
using Microsoft.Extensions.Logging;
using Navira.Shop.Core.Bus;

namespace NaviraShop.Core.Mq
{
    public class Publisher : IPublisher
    {
        private IMessageFactory _messageFactory;

        IBusControl massTransitBus;


        public Publisher(IBusControl busControl, IMessageFactory messageFactory)
        {
            massTransitBus = busControl;
            _messageFactory = messageFactory;
        }
        public async Task Publish<T>(T eventData) where T : IEvent
        {
            await massTransitBus.Publish(eventData);
        }
        public async Task Log(string message, LogLevel level)
        {
            await massTransitBus.Publish(await _messageFactory.LogMessage(message, level));
        }
        public async Task Log(string message, LogData data = null, LogLevel level = LogLevel.Information)
        {

            await massTransitBus.Publish(await _messageFactory.LogMessage(message, data, level));
        }
        public async Task Log(LogEvent log)
        {
            await massTransitBus.Publish(log);
        }
        public async Task Log(string message)
        {

            await Log(message, LogLevel.Information);
        }
        public async Task Error(string message)
        {
            await Log(message, LogLevel.Error);
        }
        public async Task Warning(string message)
        {
            await Log(message, LogLevel.Warning);
        }
        public async Task Log(Exception exception, string message = "", LogLevel level = LogLevel.Error)
        {
            await massTransitBus.Publish(await _messageFactory.LogMessage(exception, message, level));

        }




    }
}
