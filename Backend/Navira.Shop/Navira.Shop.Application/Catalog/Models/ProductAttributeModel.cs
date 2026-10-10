using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeModel : BaseReadModel<int>, IAuditableEntity
    {

        public string Name { get; set; } = default!;
        public bool IsActive { get; set; }
        public bool IsVariantAttribute { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsVisible { get; set; }
        public bool IsSearchable { get; set; }
        public byte ValueType { get; set; }
        public virtual ICollection<ProductAttributeOptionModel> ProductAttributeOption { get; set; } = new List<ProductAttributeOptionModel>();
        public virtual ICollection<ProductAttributeValueModel> ProductAttributeValue { get; set; } = new List<ProductAttributeValueModel>();
        public virtual ICollection<ProductVariantAttributeValueModel> ProductVariantAttributeValue { get; set; } = new List<ProductVariantAttributeValueModel>();


    }
}

