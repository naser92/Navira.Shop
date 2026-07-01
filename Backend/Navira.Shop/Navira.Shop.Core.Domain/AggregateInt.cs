namespace Navira.Shop.Core.Domain
{
    public abstract class AggregateInt : AggregateRoot<int>, IHasPriority
    {
        protected AggregateInt()
        {
        }

        protected AggregateInt(int id) : base(id)
        {
        }
        public int Priority { get; set; }
    }
}
