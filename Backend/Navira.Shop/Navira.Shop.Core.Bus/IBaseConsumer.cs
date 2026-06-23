using MassTransit;

namespace Navira.Shop.Core.Bus
{
    public interface IBaseConsumer<T> : IConsumer<T> where T : class
    {
    }
}
