using Navira.Shop.Core.Domain;

namespace Navira.Shop.Domain.Catalog
{
    public class ProductAttribute : FullEntity<int>, IFullAuditableEntity<Guid>
    {

        public string Name { get; set; }

        public string ValueType { get; set; }

        public string Usage { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ProductAttributeOption> ProductAttributeOption { get; set; }

        public virtual ICollection<ProductVariantAttributeValue> ProductVariantAttributeValue { get; set; }

    }
}
