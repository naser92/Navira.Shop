using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeModel : BaseReadModel<int>, IAuditableEntity
    {

        public string Name { get; set; }

        public string ValueType { get; set; }

        public string Usage { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ProductAttributeOptionModel> ProductAttributeOption { get; set; }


        public virtual ICollection<ProductVariantAttributeValueModel> ProductVariantAttributeValue { get; set; }


    }
}

