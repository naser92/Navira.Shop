namespace Navira.Shop.Core.Entity
{
    public class EntityDeletedEvent<T> : BaseCrudEvent<T>, IIntegrationEvent
    {
        public EntityDeletedEvent(T entity, Guid? doBy, DateTime? doDate) : base(entity, doBy, doDate)
        {
        }
    }
}
