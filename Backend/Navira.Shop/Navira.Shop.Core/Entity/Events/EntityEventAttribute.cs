namespace Navira.Shop.Core.Entity
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityEventAttribute : Attribute
    {
        public Type? UpdateEvent;
        public Type? DeleteEvent;
        public Type? CreateEvent;
        public Type? AllEvent;
    }
}
