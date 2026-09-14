using Navira.Shop.Application.Catalog.DTOs.ProductAttribute;
using Navira.Shop.Core.Bus;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeListCommand : ProductAttributeGetFilterDto, ICommand
    {
    }
}
