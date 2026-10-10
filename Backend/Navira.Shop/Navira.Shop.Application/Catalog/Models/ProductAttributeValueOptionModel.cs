using Navira.Shop.Application.Common;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeValueOptionModel : BaseReadModel<long>
    {
        public long ProductAttributeValueId { get; set; }
        public int ProductAttributeId { get; set; }
        public int ProductAttributeOptionId { get; set; }
        public virtual ProductAttributeValueModel ProductAttributeValue { get; set; } = default!;
        public virtual ProductAttributeOptionModel ProductAttributeOption { get; set; } = default!;
    }
}
