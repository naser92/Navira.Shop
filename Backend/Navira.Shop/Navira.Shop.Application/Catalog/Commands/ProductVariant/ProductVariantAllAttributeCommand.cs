using Navira.Shop.Core.Bus;

namespace Navira.Shop.Application.Catalog
{
    public class ProductVariantAllAttributeCommand : ICommand
    {
        public ProductVariantAllAttributeCommand(int productid)
        {
            ProductId = productid;
        }
        public int ProductId { get; set; }
    }
}
