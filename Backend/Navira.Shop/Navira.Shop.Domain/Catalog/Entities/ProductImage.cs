using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Catalog
{
    public class ProductImage : FullEntity<long>, IFullAuditableEntity<Guid>
    {

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant ProductVariant { get; set; }

        public int ProductId { get; set; }

        public int? ProductVariantId { get; set; }

        public string Url { get; set; }

        public string AltText { get; set; }

        public int SortOrder { get; set; }

        public bool IsPrimary { get; set; }

    }
}
