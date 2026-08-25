using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Catalog
{
    public class ProductVariantAttributeValueModel : BaseReadModel<long>, IAuditableEntity
    {

        public int ProductVariantId { get; set; }

        public int ProductAttributeId { get; set; }

        public int ProductAttributeOptionId { get; set; }

        [ForeignKey("ProductAttributeId")]
        public virtual ProductAttributeModel ProductAttribute { get; set; }

        [ForeignKey("ProductAttributeOptionId")]
        public virtual ProductAttributeOptionModel ProductAttributeOption { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariantModel ProductVariant { get; set; }

    }
}

