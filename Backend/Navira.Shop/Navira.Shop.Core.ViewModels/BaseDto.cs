namespace Navira.Shop.Core.ViewModels
{
    public class BaseDto<TKey> : IBaseDto<TKey>
    {
        public TKey Id { get; set; }
    }
}
