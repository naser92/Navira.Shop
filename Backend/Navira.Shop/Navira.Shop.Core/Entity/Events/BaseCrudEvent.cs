namespace Navira.Shop.Core.Entity
{
    public class BaseCrudEvent<T>
    {
        public BaseCrudEvent(T entity, Guid? doBy, DateTime? doDate)
        {
            DoDate = doDate;
            DoBy = doBy;
            Entity = entity;
        }
        public DateTime? DoDate { get; init; }
        public Guid? DoBy { get; init; }
        public T Entity { get; init; }
    }
}
