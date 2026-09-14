using Navira.Shop.Core.Domain;

namespace Navira.Shop.Domain.Catalog
{
    public class ProductAttribute : FullEntity<int>, IFullAuditableEntity<Guid>
    {

        public string Name { get; set; }

        public AttributeValueType ValueType { get; set; }
        public bool IsVariantAttribute { get; set; }
        public bool IsVisible { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsSearchable { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ProductAttributeOption> ProductAttributeOption { get; set; }

        public virtual ICollection<ProductVariantAttributeValue> ProductVariantAttributeValue { get; set; }

        public ProductAttribute() { }

        public ProductAttribute(string name, AttributeValueType valueType, bool isVariantAttribute, bool isVisible, bool isFilterable, bool isSearchable, bool isActive)
        {
            Name = name;
            ValueType = valueType;
            IsVariantAttribute = isVariantAttribute;
            IsVisible = isVisible;
            IsFilterable = isFilterable;
            IsSearchable = isSearchable;
            IsActive = isActive;
        }


        public static ProductAttribute Cretae(string name, AttributeValueType valueType, bool isVariantAttribute, bool isVisible, bool isFilterable, bool isSearchable, bool isActive)
        {
            return new ProductAttribute(name, valueType, isVariantAttribute, isVisible, isFilterable, isSearchable, isActive);
        }


    }
}
