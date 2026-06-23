namespace Navira.Shop.Core.Bus
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConsumerConfigAttribute : Attribute
    {
        public int ConcurrencyLimit { get; set; }
        public string Name { get; set; }
        public ConsumerConfigAttribute(int concurrencyLimit, string name)
        {
            ConcurrencyLimit = concurrencyLimit;
            Name = name;
        }
    }
}
