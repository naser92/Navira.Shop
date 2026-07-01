namespace Navira.Shop.Core.Entity
{
    public class EntityUpdatedEvent<T> : BaseCrudEvent<T>, IIntegrationEvent
    {
        public EntityUpdatedEvent(T entity, Guid? doBy, DateTime? doDate) : base(entity, doBy, doDate)
        {
        }

    }
}
