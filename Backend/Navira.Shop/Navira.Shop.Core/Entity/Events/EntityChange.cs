using Microsoft.EntityFrameworkCore;

namespace Navira.Shop.Core.Entity
{
    public class EntityChange
    {
        public EntityChange(IAggregateRoot entity, EntityState state, Guid? doBy, DateTime? doDate)
        {
            Entity = entity;
            State = state;
            DoBy = doBy;
            DoDate = doDate;
        }
        public IAggregateRoot Entity { get; set; }
        public EntityState State { get; set; }

        public Guid? DoBy { get; set; }
        public DateTime? DoDate { get; set; }
    }
}