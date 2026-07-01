using Microsoft.Extensions.DependencyInjection;

namespace Navira.Shop.Core.Bus
{
    public sealed class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public DomainEventDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public async Task DispatchAsync(IEnumerable<IEvent> domainEvents, CancellationToken ct = default)
        {
            foreach (var domainEvent in domainEvents)
            {
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
                foreach (var handler in _serviceProvider.GetServices(handlerType))
                {
                    var method = handlerType.GetMethod(nameof(IDomainEventHandler<IEvent>.HandleAsync))!;
                    await (Task)method.Invoke(handler, new object[] { domainEvent, ct })!;
                }
            }
        }
    }
}
