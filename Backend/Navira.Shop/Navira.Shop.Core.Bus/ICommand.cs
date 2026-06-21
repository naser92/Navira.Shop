namespace Navira.Shop.Core.Bus
{
    public interface ICommand : IMessage
    {
    }
    public class UpdateCommand<Tkey> : ICommand
    {
        public Tkey Id { get; set; }
    }
}
