namespace Navira.Shop.Core.Entity
{
    public class EntityCreatedEvent<T> : BaseCrudEvent<T>, IIntegrationEvent
    {
        public EntityCreatedEvent(T entity, Guid? doBy, DateTime? doDate) : base(entity, doBy, doDate)
        {
        }


    }
}
