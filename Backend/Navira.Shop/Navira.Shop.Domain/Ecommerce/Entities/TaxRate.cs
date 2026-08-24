using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Ecommerce
{
    public class TaxRate : FullEntity<int>, IFullAuditableEntity<Guid>
    {

        [ForeignKey("TaxCategoryId")]
        public virtual TaxCategory TaxCategory { get; set; }

        public int TaxCategoryId { get; set; }

        public decimal Percentage { get; set; }

        public DateTime EffectiveFromUtc { get; set; }

        public DateTime? EffectiveToUtc { get; set; }

    }
}
