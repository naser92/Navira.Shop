using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeOptionModel : BaseReadModel<int>, IAuditableEntity
    {

        public int ProductAttributeId { get; set; }

        public string Value { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ProductAttributeId")]
        public virtual ProductAttributeModel ProductAttribute { get; set; }

        public virtual ICollection<ProductVariantAttributeValueModel> ProductVariantAttributeValue { get; set; }


    }
}

