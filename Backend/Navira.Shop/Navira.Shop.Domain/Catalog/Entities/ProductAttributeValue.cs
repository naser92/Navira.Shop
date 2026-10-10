using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Catalog
{
    public class ProductAttributeValue : FullEntity<long>, IFullAuditableEntity<Guid>
    {
        public int ProductId { get; set; }
        public int ProductAttributeId { get; set; }
        public int? ProductAttributeOptionId { get; set; }
        public string? TextValue { get; set; }
        public decimal? NumberValue { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime? DateValue { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = default!;
        [ForeignKey(nameof(ProductAttributeId))]
        public virtual ProductAttribute ProductAttribute { get; set; } = default!;
        public virtual ProductAttributeOption? ProductAttributeOption { get; set; }
        public virtual ICollection<ProductAttributeValueOption> ProductAttributeValueOption { get; set; } = new List<ProductAttributeValueOption>();
    }
}
