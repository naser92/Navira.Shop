using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Catalog
{
    public class ProductVariantAttributeValue : FullEntity<long>, IFullAuditableEntity<Guid>
    {

        [ForeignKey("ProductAttributeId")]
        public virtual ProductAttribute ProductAttribute { get; set; }

        [ForeignKey("ProductAttributeOptionId")]
        public virtual ProductAttributeOption ProductAttributeOption { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant ProductVariant { get; set; }

        public int ProductVariantId { get; set; }

        public int ProductAttributeId { get; set; }

        public int ProductAttributeOptionId { get; set; }

    }
}
