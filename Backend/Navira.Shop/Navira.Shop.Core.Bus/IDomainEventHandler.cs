namespace Navira.Shop.Core.Bus
{
    /// <summary>
    /// Handler contract for a specific event type. Resolved via the same ITypeFinder
    /// auto-registration convention already used for ICommandHandler/IQueryHandler — add
    /// `serviceCollection.Register(typeFinder, typeof(IDomainEventHandler&lt;&gt;), ServiceLifetime.Scoped);`
    /// alongside the existing registrations in ServiceCollectionExtensions.
    /// Multiple handlers may exist for the same TEvent; all are invoked.
    /// </summary>
    public interface IDomainEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent domainEvent, CancellationToken ct = default);
    }

}
