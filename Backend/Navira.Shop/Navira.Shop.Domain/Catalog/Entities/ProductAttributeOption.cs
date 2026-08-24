using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Catalog
{
    public class ProductAttributeOption : FullEntity<int>, IFullAuditableEntity<Guid>
    {

        [ForeignKey("ProductAttributeId")]
        public virtual ProductAttribute ProductAttribute { get; set; }

        public int ProductAttributeId { get; set; }

        public string Value { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ProductVariantAttributeValue> ProductVariantAttributeValue { get; set; }

    }
}
