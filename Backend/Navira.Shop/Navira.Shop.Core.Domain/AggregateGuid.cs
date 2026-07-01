namespace Navira.Shop.Core.Domain
{
    public abstract class AggregateGuid : AggregateRoot<Guid>
    {
        protected AggregateGuid() : base(Guid.NewGuid())
        {
        }

        protected AggregateGuid(Guid id) : base(id)
        {
        }

    }
}
