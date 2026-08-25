using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Ecommerce
{
    public class TaxRateModel : BaseReadModel<int>, IAuditableEntity
    {

        public int TaxCategoryId { get; set; }

        public decimal Percentage { get; set; }

        public DateTime EffectiveFromUtc { get; set; }

        public DateTime? EffectiveToUtc { get; set; }

        [ForeignKey("TaxCategoryId")]
        public virtual TaxCategoryModel TaxCategory { get; set; }

    }
}

