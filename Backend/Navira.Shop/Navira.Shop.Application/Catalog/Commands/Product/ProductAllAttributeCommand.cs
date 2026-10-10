using Navira.Shop.Core.Bus;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAllAttributeCommand : ICommand
    {
        public int ProductId { get; set; }
    }
}
