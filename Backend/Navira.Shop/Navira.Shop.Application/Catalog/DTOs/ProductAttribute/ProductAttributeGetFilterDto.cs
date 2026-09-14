using Navira.Shop.Application.Common;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog.DTOs.ProductAttribute
{
    public class ProductAttributeGetFilterDto : PaginationModel
    {
        public string Name { get; set; }
        public AttributeValueType? ValueType { get; set; }
        public bool? IsVariantAttribute { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsFilterable { get; set; }
        public bool? IsSearchable { get; set; }
        public bool? IsActive { get; set; }
    }
}
