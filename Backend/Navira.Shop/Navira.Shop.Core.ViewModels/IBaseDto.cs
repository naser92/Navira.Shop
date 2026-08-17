namespace Navira.Shop.Core.ViewModels
{
    public interface IBaseDto<TKey>
    {
        public TKey Id { get; set; }
    }
}
