namespace Navira.Shop.Core.Domain
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<Bus.IEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }

}
