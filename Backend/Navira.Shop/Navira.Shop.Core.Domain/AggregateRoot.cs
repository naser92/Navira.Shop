using Navira.Shop.Core.Entity;

namespace Navira.Shop.Core.Domain
{
    public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot, IHasDomainEvents
    {
        private readonly List<Bus.IEvent> _domainEvents = new();

        public IReadOnlyCollection<Bus.IEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected AggregateRoot() { }

        protected AggregateRoot(TId id) : base(id) { }

        protected void Raise(Bus.IEvent domainEvent) => _domainEvents.Add(domainEvent);

        /// <summary>Called by infrastructure immediately after events have been
        /// successfully dispatched (right after IUnitOfWork.Commit() publishes them).</summary>
        public void ClearDomainEvents() => _domainEvents.Clear();
    }

}
