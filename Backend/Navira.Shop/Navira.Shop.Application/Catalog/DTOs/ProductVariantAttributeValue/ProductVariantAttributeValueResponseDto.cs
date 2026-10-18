using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog;

public class ProductVariantAttributeValueResponseDto : BaseDto<long>
{
    public string Sku { get; set; }
    public decimal Price { get; set; }
    public decimal CostPrice { get; set; }
    public bool IsActive { get; set; }
    public List<ProductVariantAllAttributeValueDto> Attributes { get; set; }
}
