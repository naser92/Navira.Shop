namespace Navira.Shop.Core.Bus
{
    /// <summary>
    /// NEW addition to Core.Bus, in support of the Domain layer. Domain aggregates raise
    /// events (as Navira.Shop.Core.Bus.IEvent — see Domain.SeedWork.AggregateRoot&lt;TId&gt;.Raise,
    /// which reuses the existing IEvent marker rather than introducing a parallel
    /// IDomainEvent type) without any reference to how they'll be handled. This is the
    /// contract infrastructure implements to actually dispatch them — see
    /// Navira.Shop.Infrastructure.Persistence.EfUnitOfWork.Commit(), the only place in the
    /// pipeline positioned to call this (BusControl calls Uow.Commit() automatically;
    /// there's no separate "after everything" hook to add this to instead).
    ///
    /// Does not replace IBus/IQueryBus — domain events are fire-and-forget notifications
    /// about something that already happened, not commands awaiting a result.
    /// </summary>
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IEvent> domainEvents, CancellationToken ct = default);
    }

}
