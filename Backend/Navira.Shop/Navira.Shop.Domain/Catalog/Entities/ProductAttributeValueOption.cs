using Navira.Shop.Core.Domain;

namespace Navira.Shop.Domain.Catalog
{
    public class ProductAttributeValueOption : FullEntity<long>, IFullAuditableEntity<Guid>
    {
        public long ProductAttributeValueId { get; set; }
        public int ProductAttributeId { get; set; }
        public int ProductAttributeOptionId { get; set; }
        public virtual ProductAttributeValue ProductAttributeValue { get; set; } = default!;
        public virtual ProductAttributeOption ProductAttributeOption { get; set; } = default!;
    }
}
