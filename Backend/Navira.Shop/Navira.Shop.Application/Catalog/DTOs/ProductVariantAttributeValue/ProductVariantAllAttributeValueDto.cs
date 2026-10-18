using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog;

public class ProductVariantAllAttributeValueDto : BaseDto<long>
{
    public long ProductVariantId { get; set; }
    public long ProductAttributeId { get; set; }
    public long ProductAttributeOptionId { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string Code { get; set; }
}
